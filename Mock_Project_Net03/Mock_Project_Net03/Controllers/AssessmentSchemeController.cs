using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentSchemeController : ControllerBase
    {
        private AssessmentSchemeService _assessmentSchemeService;
        public AssessmentSchemeController(AssessmentSchemeService assessmentSchemeService)
        {
            _assessmentSchemeService = assessmentSchemeService;
        }
        [HttpGet("GetAllAssessmentSchemes")]
        public async Task<IActionResult> GetAssessmentScheme()
        {
            try
            {
                var assessmentSchemes = await _assessmentSchemeService.GetAssessmentSchemes();
                return Ok(ApiResult<AssessmentSchemeResponse>.Succeed(new AssessmentSchemeResponse
                {
                    assessmentSchemes = assessmentSchemes
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<AssessmentSchemeResponse>.Fail(ex));
            }

        }
        [HttpGet("GetAssessmentSchemeById/{id}")]
        public async Task<IActionResult> GetAssessmentSchemelById(int id)
        {
            try
            {
                var assessmentScheme = await _assessmentSchemeService.GetAssessmentSchemeById(id);
                return Ok(ApiResult<AssessmentSchemeResponse>.Succeed(new AssessmentSchemeResponse
                {
                    assessmentSchemeModel = assessmentScheme
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<AssessmentSchemeResponse>.Fail(ex));
            }
        }
        [HttpPost("CreateAssessmentScheme")]
        public async Task<IActionResult> CreateAssessmentScheme(AssessmentScheme_ToAdd assessmentSchemeModel)
        {
            try
            {
                var assessmentScheme = await _assessmentSchemeService.CreateAssessmentScheme(assessmentSchemeModel);
                return Ok(ApiResult<AssessmentSchemeResponse>.Succeed(new AssessmentSchemeResponse
                {
                    assessmentSchemeModel = assessmentScheme
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<AssessmentSchemeResponse>.Fail(ex));

            }
        }
    }
}
