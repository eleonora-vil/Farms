using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Services.Syllabus;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Microsoft.AspNetCore.Authorization;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewTrainingCalendarController : ControllerBase
    {
        private ViewTrainingCalendarService _viewTrainingCalendarService;
        private UserService _userService;
        private PermissionService _permissionService;
        public ViewTrainingCalendarController(ViewTrainingCalendarService viewTrainingCalendarService, UserService userService, PermissionService permissionService)
        {
            _viewTrainingCalendarService = viewTrainingCalendarService;
            _userService = userService;
            _permissionService = permissionService;
        }
        [HttpGet("View-Training-Calendar")]
        public async Task<IActionResult> GetDay(DateTime startDay, DateTime endDate, int? userID)
        {
            try
            {
                if(endDate.Date < startDay.Date) {
                    return BadRequest(ApiResult<ViewTrainingCalendarResponse>.Error(new ViewTrainingCalendarResponse
                    {
                        message = "EndDate must be after StartDay"
                    }));
                }
                var viewTrainingCalendars = await _viewTrainingCalendarService.GetDay(startDay, endDate, userID);
                return Ok(ApiResult<ViewTrainingCalendarResponse>.Succeed(new ViewTrainingCalendarResponse
                {
                    viewTrainingCalendarModel = viewTrainingCalendars
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<ViewTrainingCalendarResponse>.Fail(ex));
            }
        }
    }
}
