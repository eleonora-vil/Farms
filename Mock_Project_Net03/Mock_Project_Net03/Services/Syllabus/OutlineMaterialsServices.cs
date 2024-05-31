using AutoMapper;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services.Syllabus
{
    public class OutlineMaterialsServices
    {
        private readonly IRepository<Materials, int> _materialRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<LearningObj, int> _learningObjRepo;

        public OutlineMaterialsServices(IRepository<Materials, int> materialRepo, IMapper mapper, IRepository<LearningObj, int> learningObjRepo)
        {
            _materialRepo = materialRepo;
            _mapper = mapper;
            _learningObjRepo = learningObjRepo;
        }
        public async Task<MaterialsResponse> CreateMaterials(MaterialsResponse newMaterial)
        {
            var materials = _mapper.Map<Materials>(newMaterial);
            if (newMaterial.LearningObjId <= 0)
            {
                throw new BadRequestException("Invalid LearningObjId");
            }
            var learningDomain = _learningObjRepo.FindByCondition(x => x.LearningObjId == newMaterial.LearningObjId).FirstOrDefault();
            if (learningDomain == null)
            {
                throw new BadRequestException("LearningObjId does not exist");
            }
            var classMaterialsEntity = new Materials
            {
                Name = newMaterial.Name,
                CreateBy = newMaterial.CreateBy,
                CreateDate = newMaterial.CreateDate,
            };
            classMaterialsEntity.LearningObjId = learningDomain.LearningObjId;
            classMaterialsEntity.LearningObjId = learningDomain.LearningObjId;
            classMaterialsEntity = await _materialRepo.AddAsync(classMaterialsEntity);
            var result = await _materialRepo.Commit();
            return newMaterial;

        }
    }
}
