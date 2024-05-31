using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ClassService _classService;

        public InstructorController (ClassService classService)
        {
            _classService = classService;
        }

        [HttpPost("SearchFreeInstructor")]
        public async Task<IActionResult> SearchFreeInstructor([FromBody] CheckFreeIntructorModel req)
        {
            if (req.Slot < 1 ||  req.Slot > 3)
            {
                return BadRequest(ApiResult<GetInstructorsResponse>.Error(new GetInstructorsResponse
                {
                    message = "Invalid Slot"
                }));
            }

            var result = await _classService.SearchFreeInstructors(req);

            if (result == null)
            {
                return BadRequest(ApiResult<GetInstructorsResponse>.Error(new GetInstructorsResponse
                {
                    message = "Invalid Request"
                }));
            }
            else
            {
                return Ok(ApiResult<GetInstructorsResponse>.Succeed(new GetInstructorsResponse
                {
                    Users = result,
                    message = "Invalid Request"
                }));
            }
        }
    }
}
