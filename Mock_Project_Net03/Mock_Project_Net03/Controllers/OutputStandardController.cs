using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using System.Drawing.Printing;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputStandardController : ControllerBase
    {
        private OutputStandardService _outputStandardService;
        public OutputStandardController(OutputStandardService outputStandardService)
        {
            _outputStandardService = outputStandardService;
        }
        [HttpGet("GetAllOutputStandards")]
        public async Task<IActionResult> GetAllOutPutStandardBy()
        {
            try
            {
                var outputStandards = await _outputStandardService.GetAllOutPutStandards();
                return Ok(ApiResult<OutputStandardResponse>.Succeed(new OutputStandardResponse
                {
                    outputStandardModels = outputStandards
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<OutputStandardResponse>.Fail(ex));
            }
        }
        [HttpGet("GetOutPutStandardById/{id}")]
        public async Task<IActionResult> GetOutPutStandardById(int id)
        {
            try
            {
                var outputStandard = await _outputStandardService.GetOutPutStandardById(id);
                return Ok(ApiResult<OutputStandardResponse>.Succeed(new OutputStandardResponse
                {
                    outputStandardModel = outputStandard
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<OutputStandardResponse>.Fail(ex));
            }
        }
        [HttpPost("CreateOutPutStandard")]
        public async Task<IActionResult> CreateOutPutStandard(OutputStandardModel outputStandardModel)
        {
            try
            {
                var outputStandard = await _outputStandardService.CreateOutPutStandard(outputStandardModel);
                return Ok(ApiResult<OutputStandardResponse>.Succeed(new OutputStandardResponse
                {
                    outputStandardModel = outputStandard
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<OutputStandardResponse>.Fail(ex));
            }
        }
    }
}
