using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
namespace Mock_Project_Net03.Services.Syllabus
{
    public class SyllabusOutlineLearningObjServices
    {
        private readonly string _unitName;
        private readonly IRepository<LearningObj, int> _learningObjRepo;
        private readonly IRepository<OutputStandard, int> _outputStandardRepo;
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepo;
        private readonly IRepository<Entities.Materials, int> _materialsRepository;
        private IMapper _mapper;

        public SyllabusOutlineLearningObjServices(IRepository<LearningObj, int> LearningObj,
            IRepository<OutputStandard, int> OutputStandard,
            IRepository<TrainingProgramUnit, int> TrainProU,
            IRepository<Entities.Materials, int> MaterialsRepository,
            IMapper mapper)
        {
            _learningObjRepo = LearningObj;
            _outputStandardRepo = OutputStandard;
            _trainingProgramUnitRepo = TrainProU;
            _materialsRepository = MaterialsRepository;
            _mapper = mapper;
        }
        public async Task<LearningObjResponse> CreateLearningObj(LearningObjResponse newLearningObj)
        {
            var learningObj = _mapper.Map<LearningObj>(newLearningObj);
            if (newLearningObj.UnitId <= 0 || newLearningObj.OutputStandardId <= 0)
            {
                throw new BadRequestException("Invalid OutputStandardId or UnitId");
            }
            var outputStandardIdExits = _outputStandardRepo.FindByCondition(x => x.OutputStandardId == newLearningObj.OutputStandardId).FirstOrDefault();
            if (outputStandardIdExits == null)
            {
                throw new BadRequestException("OutputStandard does not exist!");
            }
            var unitIdEntity = _trainingProgramUnitRepo.FindByCondition(x => x.UnitId == newLearningObj.UnitId).FirstOrDefault();
            if (unitIdEntity is null)
            {
                throw new BadRequestException("UnitId does not exist!");
            }


            var classLearningObjEntity = new LearningObj
            {
                Name = newLearningObj.Name,
                OutputStandardId = newLearningObj.OutputStandardId,
                TrainningTime = newLearningObj.TrainningTime,
                Method = newLearningObj.Method,
                Index = 0,
            };
            classLearningObjEntity.UnitId = unitIdEntity.UnitId;
            classLearningObjEntity = await _learningObjRepo.AddAsync(classLearningObjEntity);
            var result = await _learningObjRepo.Commit();
            return newLearningObj;
        }


        public async Task<LearningObjResponse> GetOutlineByUnitId(int unitId)
        {
            var learningObj = await _learningObjRepo.FindByCondition(x => x.UnitId == unitId)
                                                          .FirstOrDefaultAsync();
            if (learningObj != null)
            {
                var materials = await _materialsRepository.FindByCondition(x => x.LearningObjId == learningObj.LearningObjId)
                                                           .ToListAsync();
                var learningObjModel = new LearningObjResponse
                {
                    UnitId = unitId,
                    Name = learningObj.Name,
                    LearningObjId = learningObj.LearningObjId,
                    TrainningTime = (DateTime)learningObj.TrainningTime,
                    Index = (int)learningObj.Index,
                    OutputStandardId = learningObj.OutputStandardId,
                    Material = materials.Select(learningObjId => new MaterialsResponse
                    {
                        MaterialsId = learningObjId.MaterialsId,
                        Name = learningObjId.Name,
                        CreateBy = learningObjId.CreateBy,
                        CreateDate = learningObjId.CreateDate
                    }).ToList()
                };

                return learningObjModel;
            }

            return null;
        }





    }
}
