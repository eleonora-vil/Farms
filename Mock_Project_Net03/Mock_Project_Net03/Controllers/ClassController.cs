using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Common.Payloads.Requests;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private ClassService _classService;
        public ClassController(ClassService classService)
        {
            _classService = classService;
        }

        [Authorize]
        [HttpGet("SearchClass/{pageNumber}")]
        public async Task<ActionResult<GetClassListResponse>> GetAllClassWithFilter(
            int pageNumber,
            int pageSize,
            string? KeyWord,
//            string? ClassLocation,
            string? ClassTime,
            string? Status,
            DateTime TimeFrom,
            DateTime TimeTo,
            string? FSU,
            int TrainerId
            )
        {
            try
            {
                var (result, totalPages) = await _classService.GetAllClass(pageNumber, pageSize);
                var resultWithFilter = await _classService.GetAllClassWithFilter(
                    result, 
                    KeyWord, 
//                    ClassLocation, 
                    ClassTime, 
                    Status,
                    TimeFrom, 
                    TimeTo, 
                    FSU, 
                    TrainerId);

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
            var res = await _classService.GetClassById(id);
            if (res is null)
            {
                return Ok(ApiResult<GetClassByIdResponse>.Succeed(new GetClassByIdResponse
                {
                    Class = res,
                    Message = "Class not found!"
                }));
            }
            return Ok(ApiResult<GetClassByIdResponse>.Succeed(new GetClassByIdResponse
            {
                Class = res,
                Message = null
            }));
        }

        [Authorize]
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<GetClassDetailResponse>> GetClassDetail(int id)
        {
            var res = await _classService.GetClassDetail(id);
            if (res is null)
            {
                return NotFound(ApiResult<GetClassDetailResponse>.Fail(new NotFoundException("Class not found!")));
            }
            return Ok(ApiResult<GetClassDetailResponse>.Succeed(res));
        }
    }
}
