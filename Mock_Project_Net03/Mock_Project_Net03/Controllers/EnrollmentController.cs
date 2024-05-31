using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Microsoft.AspNetCore.Authorization;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Entities;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {

        private EnrollmentService _enrollmentService;
        public EnrollmentController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("GetAllUserEnrollInClass/{classId}")]
        [Authorize]
        public IActionResult GetAllUserEnrollInClass(int classId)
        {
            var users = _enrollmentService.GetAllUserEnrollInClass(classId);
            if (users is not null)
            {
                return Ok(ApiResult<ListUserResponse>.Succeed(new ListUserResponse
                {
                    Users = users,
                }));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("RemoveStudentInClass/{enrollmentId}")]
        [Authorize(Roles = "Super Admin, Admin, Instructor")]
        public async Task<IActionResult> RemoveStudentInClass(int enrollmentId)
        {
            var action = await  _enrollmentService.RemoveEnrolledStudent(enrollmentId);
            if (action)
            {
                return Ok(ApiResult<RemoveEnrollmentedStudentResponse>.Succeed(new RemoveEnrollmentedStudentResponse
                {
                    message = "Remove sucessfully",
                }));
            }
            return BadRequest(ApiResult<RemoveEnrollmentedStudentResponse>.Error(new RemoveEnrollmentedStudentResponse
            {
                message = "Something went wrong!",
            }));
        }



        [HttpPost("Add-Student-To-Class")]
       // [Authorize]
        public async Task<IActionResult> AddStudentToClass(EnrollmentRequest req)
        {
            try
            {
                var addStudent = await _enrollmentService.AddStudentToClass(req);
                if (addStudent is not null)
                {
                    return Ok(ApiResult<CreateEnrollmentResponse>.Succeed(new CreateEnrollmentResponse
                    {
                        enrollmentModel = addStudent,
                        message = "Create successfully!"
                    }));
                }
                else
                {
                    return BadRequest(ApiResult<CreateEnrollmentResponse>.Succeed(new CreateEnrollmentResponse
                    {
                        enrollmentModel = null,
                        message = "Create failed!"
                    }));
                }
            }
            catch (Exception ex) 
            {
             
                return BadRequest(ApiResult<CreateEnrollmentResponse>.Fail(ex));
        }
        }

    }
}
