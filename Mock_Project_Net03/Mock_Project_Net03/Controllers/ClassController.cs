using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Entities;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Mock_Project_Net03.Validation;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private ClassService _classService;
        private UserService _userService;
        private readonly PermissionService _permissionService;

        public ClassController(ClassService classService, UserService userService, PermissionService permissionService)
        {
            _classService = classService;
            _userService = userService;
            _permissionService = permissionService;
        }

        [Authorize]
        [HttpGet("SearchClass/{pageNumber}")]
        public async Task<ActionResult<GetClassListResponse>> GetAllClassWithFilter(
            int pageNumber,
            int pageSize,
            string? KeyWord,
            string? Status,
            string? Semester,
            int TrainerId
            )
        {
            try
            {
                var (result, totalPages) = await _classService.GetAllClass(pageNumber, pageSize);
                var resultWithFilter = await _classService.GetAllClassWithFilter(
                    result, 
                    KeyWord, 
                    Status,
                    Semester, 
                    TrainerId
                    );

                return Ok(ApiResult<GetClassListResponse>.Succeed(new GetClassListResponse
                {
                    Classes = resultWithFilter,
                    TotalPages = totalPages,
                    Message = null
                }));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResult<GetClassListResponse>.Fail(ex));
            }
            catch (NotFoundException ex)
            {
                return Ok(ApiResult<GetClassListResponse>.Succeed(new GetClassListResponse
                {
                    Classes = null,
                    TotalPages = 0,
                    Message = ex.Message
                }));
            }
        }

        [Authorize]
        [HttpGet("GetAllClass/{pageNumber}")]
        public async Task<ActionResult<GetClassListResponse>> GetAllClass(int pageNumber, int pageSize)
        {
            try
            {
                var (result, totalPages) = await _classService.GetAllClass(pageNumber, pageSize);
                return Ok(ApiResult<GetClassListResponse>.Succeed(new GetClassListResponse
                {
                    Classes = result,
                    TotalPages = totalPages,
                    Message = null
                }));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResult<GetClassListResponse>.Fail(ex));
            }
            catch (NotFoundException ex)
            {
                return Ok(ApiResult<GetClassListResponse>.Succeed(new GetClassListResponse
                {
                    Classes = null,
                    TotalPages = 0,
                    Message = ex.Message
                }));
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClassByIdResponse>> GetClassById(int id)
        {
            try
            {
                var res = await _classService.GetClassById(id);
                return Ok(ApiResult<GetClassByIdResponse>.Succeed(new GetClassByIdResponse
                {
                    Class = res,
                    Message = null
                }));
            }
            catch(NotFoundException ex)
            {
                return Ok(ApiResult<GetClassByIdResponse>.Succeed(new GetClassByIdResponse
                {
                    Class = null,
                    Message = ex.Message
                }));
            }
        }

        [Authorize]
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<GetClassDetailResponse>> GetClassDetail(int id)
        {
            try
            {
                var res = await _classService.GetClassDetail(id);
                return Ok(ApiResult<GetClassDetailResponse>.Succeed(res));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResult<GetClassDetailResponse>.Fail(ex));
            }
        }

        [Authorize]
        [HttpPost("AddTrainingProgram/{classId}")]
        public async Task<ActionResult<AddTrainingProgramResponse>> AddTrainingProgramToClass(int classId, int trainingProgramId, int instructorId, int roomId, int slot)
        {
            try
            {
                var classModel = await _classService.AddTrainingProgramToClass(classId, trainingProgramId, instructorId, roomId, slot);
                return Ok(ApiResult<ClassModel>.Succeed(classModel));
//                return Ok(ApiResult<AddTrainingProgramResponse>.Succeed(new AddTrainingProgramResponse
//                {
//                    // general information for user to know which class they are adding TP
//                    ClassName = classModel.ClassName,
//                    ProgramId = classModel.ProgramId,
//                    StartDate = classModel.StartDate,
//                    EndDate = classModel.EndDate,
//                    Time = classModel.Time,
//                    // content 
//                    ClassTrainingUnit = classTrainingUnit
//                }));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResult<AddTrainingProgramResponse>.Fail(ex));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResult<AddTrainingProgramResponse>.Fail(ex));
            }
        }    


        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateNewClass([FromBody] ClassRequest classRequest)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
                // if (!permission.UserAccess.Equals("Create") && !permission.UserAccess.Equals("Full access"))
                // {
                //     throw new BadRequestException("This account do not have permission");
                // }
            var classReq = classRequest.ToClassModel();
            ClassValidator validations = new ClassValidator();
            var valid = await validations.ValidateAsync(classReq);
            if (!valid.IsValid)
            {
                throw new RequestValidationException(valid.ToProblemDetails());
            }

            var result = await _classService.CreateNewClass(classReq);
            /*            result.UserRole = userRole;*/
            if (result is not null)
            {
                return Ok(ApiResult<CreateClassRespone>.Succeed(new CreateClassRespone
                {
                    Class = result,
                }));
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (status.ToLower().Contains("active")
                || status.ToLower().Contains("deactive"))
            {
                var updateStatus = _classService.UpdateStatusClass(id, status);
                if (updateStatus != null)
                {
                    var message = $"Update status successful for ClassId: {updateStatus.Result.ClassId} with Status: {updateStatus.Result.Status}";
                    return Content(message);
                }
                return BadRequest(ApiResult<UpdateTrainingProgramResponse>.Error(new UpdateTrainingProgramResponse
                {
                    message = "Can't handle this request!"
                }));
            }
            return Content("Input status must be: Active or Deactive!");
        }

        [HttpGet("GetClassSchedule/{classId}")]
        public async Task<IActionResult> GetClassSchedule(int classId)
        {
            var classSchedules = await _classService.GetClassSchedules(classId);

            if (classSchedules == null || !classSchedules.Any())
            {
                return NotFound(); 
            }

            var response = new GetClassScheduleResponse()
            {
                ClassSchedules = classSchedules
            };

            return Ok(ApiResult<GetClassScheduleResponse>.Succeed(response));
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        public async Task<IActionResult> UpdateClass([FromRoute] int id, [FromBody] UpdateClassRequest req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            // if (!permission.UserAccess.Equals("Modify") && !permission.UserAccess.Equals("Full access"))
            // {
            //     throw new BadRequestException("This account do not have permission");
            // }

            if (!_classService.IsStarted(id))
            {
                throw new BadRequestException("This class has been started. Can not be updated");
            }
            var result = await _classService.UpdateClass(req, id);
            return Ok(ApiResult<UpdateClassResponse>.Succeed(new UpdateClassResponse()
            {
                UpdatedClass = result
            }));
        }

        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        [HttpPost("AddAttendee")]
        public async Task<IActionResult> AddAttendeeToClass(int classId, int attendeeId)
        {
            try
            {
                var result = await _classService.AddAttendeeToClass(classId, attendeeId);
                return Ok(ApiResult<AddAttendeeResponse>.Succeed(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResult<AddAttendeeResponse>.Fail(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<AddAttendeeResponse>.Fail(ex));
            }
        }

        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            try
            {
                var result = await _classService.DeleteClass(classId);
                return Ok(ApiResult<string>.Succeed(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResult<string>.Fail(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex));
            }
        }

        [Authorize]
        [HttpGet("SearchTrainingCalendar")]
        public async Task<IActionResult> SearchTrainingCalendar(string? keyword, string? roomId, DateTime? startDate, DateTime? endDate, string? shift, string? status, string? FSU, int? trainerId)
        {
            try
            {
                var result = await _classService.SearchTrainingCalendar(keyword, roomId, startDate, endDate, shift, status, FSU, trainerId);
                return Ok(ApiResult<ViewTrainingCalendarResponse>.Succeed(new ViewTrainingCalendarResponse
                {
                    viewTrainingCalendarModel = result
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<ViewTrainingCalendarResponse>.Fail(ex));
            }
        }

        [Authorize]
        [HttpPut("UpdateTrainingCalendar")]
        public async Task<IActionResult> UpdateTrainingCalendar(int classTrainingUnitId, bool applyToAll, ClassTime shift)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("Update") && !permission.UserAccess.Equals("Full access"))
            {
                return BadRequest("This account does not have permission");
            }
            try
            {
                var result = await _classService.UpdateTrainingCalendar(classTrainingUnitId, applyToAll, shift);
                return Ok(ApiResult<string>.Succeed(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResult<string>.Fail(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex));
            }
        }
    }
}
