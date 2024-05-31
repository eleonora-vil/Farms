using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Services;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private SyllabusService _syllabusService;
        private UserService _userService;
        private PermissionService _permissionService;
        public SyllabusController(SyllabusService syllabusService, UserService userService, PermissionService permissionService)
        {
            _syllabusService = syllabusService;
            _userService = userService;
            _permissionService = permissionService;
        }

        [HttpGet("GetAllSyllaBus")]
        [Authorize]
        public async Task<IActionResult> GetAllSyllabus(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (result, totalPages) = await _syllabusService.GetAllSyllabusAsync(pageNumber, pageSize);
                return Ok(ApiResult<GetSyllabusResponse>.Succeed(new GetSyllabusResponse
                {
                    Syllabus = result,
                    ToTalPages = totalPages
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<GetSyllabusResponse>.Fail(ex));
            }
        }

        [HttpGet("{syllabusId}")]
        public async Task<ActionResult<GetDetailsSyllabus>> GetSyllabusById(int syllabusId)
        {
            var (syllabus, outPutStandard) = await _syllabusService.getSyllabusById(syllabusId);
            try
            {
                return Ok(ApiResult<GetDetailsSyllabus>.Succeed(new GetDetailsSyllabus
                {
                    getSyllabusResponse = syllabus,
                    outputStandardModels = outPutStandard
                })) ;
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<GetDetailsSyllabus>.Fail(ex));
            }
        }
        

        [Authorize(Roles = "Admin, Super Admin, Instructor")]
        [HttpPost("Create")]
        public async Task<ActionResult<SyllabusModel>> PostSyllabusGeneral([FromBody] CreateSyllabusGeneralRequest createSyllabusGeneral)
        {
            if (_syllabusService == null)
            {
                return Problem("Entity set 'AppDbContext.Syllabuses'  is null.");
            }

            var res = await _syllabusService.CreateSyllabus(createSyllabusGeneral.ToSyllabusModel());

            return CreatedAtAction(nameof(GetSyllabusById), new { syllabusId = res.SyllabusId }, res);
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> SearchSyllabus([FromQuery] string keyword)
        {
            try
            {
                var result = _syllabusService.SearchSyllabusAsync(keyword.ToUpper());
                return Ok(ApiResult<GetSyllabusResponse>.Succeed(new GetSyllabusResponse
                {
                    Syllabus = result
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<GetSyllabusResponse>.Fail(ex));
            }
        }


        [Authorize(Roles = "Admin, Super Admin, Instructor")]
        [HttpPut("Update/{syllabusId}")]
        public async Task<IActionResult> UpdateSyllabus(int syllabusId, [FromBody] CreateSyllabusModel req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            // if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
            // { 
            //     throw new BadRequestException("This account do not have permission");
            // }

            if (syllabusId <= 0)
            {
                return BadRequest(ApiResult<UpdateSyllabusRespone>.Error(new UpdateSyllabusRespone
                {
                    message = "Invalid Syllabus Id"
                }));
            }

            var existingSyllabus = await _syllabusService.GetUpdateSyllabusById(syllabusId);

            if (existingSyllabus == null)
            {
                return BadRequest(ApiResult<UpdateSyllabusRespone>.Error(new UpdateSyllabusRespone
                {
                    message = "Syllabus not found"
                }));
            }

            var updatedSyllabus = await _syllabusService.UpdateSyllabus(existingSyllabus, req);

            if (updatedSyllabus != null)
            {
                return Ok(ApiResult<UpdateSyllabusRespone>.Succeed(new UpdateSyllabusRespone
                {
                    message = "Syllabus updated"
                }));
            }
            else
            {
                return BadRequest(ApiResult<UpdateSyllabusRespone>.Error(new UpdateSyllabusRespone
                {
                    message = "Invalid request"
                }));
            }
        }
        
        [Authorize(Roles = "Admin, Super Admin, Instructor")]
        [HttpPut("Duplicate/{syllabusId:int}")]
        public async Task<IActionResult> DuplicateSyllabus([FromRoute] int syllabusId)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            // if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
            // {
            //     return Forbid("This account does not have permission");
            // }
            var id = await _syllabusService.DuplicateSyllabus(syllabusId);
            return Ok(ApiResult<DuplicateSyllabusResponse>.Succeed(new DuplicateSyllabusResponse
            {
                SyllabusId = id,
            }));
        }
        
        [Authorize(Roles = "Admin, Super Admin, Instructor")]
        [HttpPut("UpdateStatus/{syllabusId}")]
        public async Task<IActionResult> UpdateSyllabusStatus(int syllabusId, [FromBody] UpdateSyllabusStatusRequest req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            // if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
            // { 
            //     return Forbid("This account do not have permission");
            // }

            var updatedSyllabus = await _syllabusService.UpdateSyllabusStatus(syllabusId, req.Status);

            return Ok(ApiResult<UpdateSyllabusRespone>.Succeed(new UpdateSyllabusRespone
            {
                message = "Syllabus status updated",
            }));
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<List<GetSyllabusByIdResponse>>> GetSyllabusByUserId(int userId)
        {
            var syllabusList = await _syllabusService.getSyllabusByUserId(userId);
            if (syllabusList == null || syllabusList.Count == 0)
            {
                return NotFound(ApiResult<UpdateSyllabusRespone>.Error(new UpdateSyllabusRespone
                {
                    message = "Syllabus not found"
                }));
            }
            return syllabusList;
        }

        [HttpGet("get/allSyllbusOftrainee/{userId}")]
        public async Task<IActionResult> getAllSyllabusByUserId(int userId)
        {
            try
            {
                var result = await _syllabusService.getAllSyllabusByUserId(userId);
                return Ok(ApiResult<getAllSyllabusByUserIdRespones>.Succeed(new getAllSyllabusByUserIdRespones
                {
                    Syllabus = result,
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<getAllSyllabusByUserIdRespones>.Fail(ex));
            }
        }

        [Authorize(Roles = "Admin, Super Admin, Instructor")]
        [HttpPost("import")]
        public async Task<IActionResult> ImportSyllabus([FromForm] IFormFile file)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var stream = file.OpenReadStream();
            var ok = await _syllabusService.ImportSyllabus(stream, currentUser.UserId);
            
            return Ok(ok);
        }
    }
}

