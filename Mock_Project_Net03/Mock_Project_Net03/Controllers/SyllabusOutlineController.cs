using MessagePack.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Services.Syllabus;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusOutlineController : ControllerBase
    {
        private SyllabusOutlineLearningObjServices _syllabusOutlineLearningObjServices;
        private SyllabusOutlineUnitServices _syllabusOutlineUnitServices;
        private OutlineMaterialsServices _services;
        

        public SyllabusOutlineController(SyllabusOutlineLearningObjServices syllabusOutlineLearningObjServices, SyllabusOutlineUnitServices syllabusOutlineUnitServices, OutlineMaterialsServices materialsServices)
        {
            _syllabusOutlineLearningObjServices = syllabusOutlineLearningObjServices
                ?? throw new ArgumentNullException(nameof(syllabusOutlineLearningObjServices));
            _syllabusOutlineUnitServices = syllabusOutlineUnitServices
                ?? throw new ArgumentNullException(nameof(syllabusOutlineUnitServices));
            _services = materialsServices
                ?? throw new ArgumentNullException(nameof(materialsServices));
        }
        [HttpPost]
        [Route("CreateUnit")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOutlineUnit([FromBody] SyllabusOutlineRequest req)
        {
                var raw_unit = req.ToUnitModel();
                var unitResult = await _syllabusOutlineUnitServices.CreateSyllabusUnit(raw_unit);
                if ( unitResult != null)
                {
                    return Ok(ApiResult<CreateSyllabusOutlineResponse>.Succeed(new CreateSyllabusOutlineResponse
                    {
                        Unit = raw_unit,
                    }));
                }
                else
                {
                    return BadRequest(ApiResult<CreateSyllabusOutlineResponse>.Error(new CreateSyllabusOutlineResponse
                    {
                        message = "Can't handle this request!"
                    }));
                }
        }
        [HttpPost]
        [Route("CreateLearningObj")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOutlineLearningObj([FromBody] OutlineLearningObjRequest req)
        {
                var raw_learningObj = req.ToLearningObjModel();
                var objResult = await _syllabusOutlineLearningObjServices.CreateLearningObj(raw_learningObj);
                if (objResult != null)
                {
                    return Ok(ApiResult<CreateLearningObjResponse>.Succeed(new CreateLearningObjResponse
                    {
                        LearningObj = raw_learningObj,
                    }));
                }
                else
                {
                    return BadRequest(ApiResult<CreateSyllabusOutlineResponse>.Error(new CreateSyllabusOutlineResponse
                    {
                        message = "Can't handle this request!"
                    }));
                }

        }
        [HttpPost]
        [Route("CreateMaterials")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMaterial([FromBody] MaterialsRequest req)
        {
                var raw_materials = req.ToMaterialsModel();
                var materialResult = await _services.CreateMaterials(raw_materials);
                if (materialResult != null)
                {
                    return Ok(ApiResult<CreateMaterialResponse>.Succeed(new CreateMaterialResponse
                    {
                        Materials = raw_materials
                    }));
                }
                else
                {
                    return BadRequest(ApiResult<CreateSyllabusOutlineResponse>.Error(new CreateSyllabusOutlineResponse
                    {
                        message = "Can't handle this request!"
                    }));
                }


        }
        [HttpGet("{unitId}")]
        public async Task<ActionResult<LearningObjResponse >> GetOutlineByUnitId(int unitId)
        {
            var learningObj= await _syllabusOutlineLearningObjServices.GetOutlineByUnitId(unitId);
            if (learningObj == null)
            {
                return NotFound("LearningObj not found");
            }
            return learningObj;
        }
    }
}
