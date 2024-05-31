using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CloudService cloudService;
        private readonly IRepository<Materials, int> materialsRepository;

        public UploadController(CloudService cloudService, IRepository<Materials, int> materialsRepository)
        {
            this.cloudService = cloudService;
            this.materialsRepository = materialsRepository;
        }
        [HttpPut("UploadFile/{id}")]
        public async Task<IActionResult> UpdateInfo(int id, [FromForm] UploadFileRequest req)
        {
            var materials = await materialsRepository.FindByCondition(x => x.MaterialsId == id).FirstOrDefaultAsync();

            if (materials == null)
            {
                return BadRequest(ApiResult<UploadRespose>.Error(new UploadRespose
                {
                    Message = "Materials is not found"
                }));
            }

            if (req.File == null)
            {
                return BadRequest(ApiResult<UploadRespose>.Error(new UploadRespose
                {
                    Message = "File is required"
                }));
            }

            var uploadFile = await cloudService.UploadImageAsync(req.File);

            

            if (uploadFile.Error == null)
            {
                materials.Url = uploadFile.SecureUrl.ToString();

                materialsRepository.Update(materials);

                await materialsRepository.Commit();

                // Return success response
                return Ok(ApiResult<UploadRespose>.Succeed(new UploadRespose
                {
                    Url = uploadFile.SecureUrl.ToString(),
                    Message = "Upload file success"
                }));
            }
            else
            {
                // Return error response
                return BadRequest(ApiResult<UploadRespose>.Error(new UploadRespose
                {
                    Message = "Upload file error"
                }));
            }
        }

    }
}
