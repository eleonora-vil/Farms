using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Services.Syllabus;
using System.Drawing.Printing;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateFullSyllabusController : ControllerBase
    {
        private CreateFullSyllabusService _createFullSyllabusService;
        private UserService _userService;
        private PermissionService _permissionService;
        public CreateFullSyllabusController(CreateFullSyllabusService createFullSyllabusService, UserService userService, PermissionService permissionService)
        {
            _createFullSyllabusService = createFullSyllabusService;
            _userService = userService;
            _permissionService = permissionService;
        }

        [Authorize(Roles = "Admin, Super Admin")]
        [HttpPost("CreateFullSyllabus")]
        public async Task<IActionResult> CreateFullSyllabus(CreateSyllabusModel createSyllabusModel)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }

            try
            {
                var result = await _createFullSyllabusService.CreateFullSyllabus(createSyllabusModel);
                if(result == null)
                {
                    return BadRequest(ApiResult<CreateFullSyllabusResponse>.Error(new CreateFullSyllabusResponse
                    {
                        message = "Status must be Active or Draft!"
                    }));
                }
                return Ok(ApiResult<CreateFullSyllabusResponse>.Succeed(new CreateFullSyllabusResponse
                {
                    message = "Succesfully created syllabus!"
                })) ;
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<CreateFullSyllabusResponse>.Fail(ex));
            }
        }

      /*  [Authorize(Roles = "Admin, Super Admin")]
        [HttpPost("CreateFullSyllabusDraft")]
        public async Task<IActionResult> CreateFullSyllabusDraft(CreateSyllabusModel createSyllabusModel)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.SyllabusAccess.Equals("Modify") && !permission.SyllabusAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }

            try
            {
                var result = await _createFullSyllabusService.CreateFullSyllabusDraft(createSyllabusModel);
                return Ok(ApiResult<CreateFullSyllabusResponse>.Succeed(new CreateFullSyllabusResponse
                {
                    message = "Succesfully created draft syllabus!"
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<CreateFullSyllabusResponse>.Fail(ex));
            }
        }*/
    }
}
