using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Services;

namespace Mock_Project_Net03.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SemesterController : Controller
{
    private readonly SemesterService _semesterService;
    public SemesterController(SemesterService semesterService)
    {
        _semesterService = semesterService;
    }

    [Authorize(Roles = "Super Admin, Admin, Instructor")]
    [HttpGet]
    public async Task<IActionResult> GetAllSemester()
    {
        var result = _semesterService.GetAllSemester();
        return Ok(ApiResult<GetAllSemesterResponse>.Succeed(new GetAllSemesterResponse()
        {
            Semesters = result
        }));
    }
}
