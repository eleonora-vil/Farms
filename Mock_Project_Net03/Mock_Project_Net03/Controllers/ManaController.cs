using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Services;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManaController : ControllerBase
    {
        private readonly ManaService _manaService;
        public ManaController(ManaService manaService)
        {
            _manaService = manaService;
        }


        [HttpGet()]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetTotalAllToMana()
        {
            var result = await _manaService.GetTotalAllToMana();
            return Ok(ApiResult<ManaResponse>.Succeed(new ManaResponse
            {
                manaModel = result,
            }));
        }
    }
}
