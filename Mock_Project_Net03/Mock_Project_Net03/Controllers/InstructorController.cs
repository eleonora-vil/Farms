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

        [HttpGet("SearchFreeInstructor")]
        public async Task<IActionResult> SearchFreeInstructor(int slot, DateTime day)
        {
            if (slot < 1 ||  slot > 3)
            {
                return BadRequest(ApiResult<GetInstructorsResponse>.Error(new GetInstructorsResponse
                {
                    message = "Invalid Slot"
                }));
            }

            var result = await _classService.SearchFreeInstructors(slot, day);

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
                    message = "Succeed"
                }));
            }
        }
    }
}
