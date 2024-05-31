using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Responses.SyllabusResonse;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services.Syllabus;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;

namespace Mock_Project_Net03.Services
{
    public class SyllabusService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly CreateFullSyllabusService _createFullSyllabusService;
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository;
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository;
        private readonly IRepository<TrainingProgram_Syllabus, int> _trainingProgramSyllabusRepository;
        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository;
        private readonly IRepository<AssessmentScheme_Syllabus, int> _assessmentSchemeSyllabusRepository;
        private readonly IRepository<LearningObj, int> _learningObjRepository;
        private readonly IRepository<Materials, int> _materialsRepository;
        private readonly IRepository<OutputStandard, int> _outputStandardRepository;
        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IMapper _mapper;
        private IRepository<Entities.Syllabus, int> syllabusRepository;
        private IMapper mapper;

        public SyllabusService(IRepository<User, int> userRepository,
            CreateFullSyllabusService createFullSyllabusService,
            IRepository<TrainingProgramUnit, int> trainingProgramUnitRepository,
            IRepository<TrainingProgram, int> trainingProgramRepository,
            IRepository<TrainingProgram_Syllabus, int> trainingProgramSyllabusRepository,
            IRepository<AssessmentScheme, int> assessmentSchemeRepository,
            IRepository<AssessmentScheme_Syllabus, int> assessmentSchemeSyllabusRepository,
            IRepository<LearningObj, int> learningObjRepository,
            IRepository<Materials, int> materialRepository,
            IRepository<OutputStandard, int> outputStandardRepository,
            IRepository<Entities.Syllabus, int> syllabusRepository,
            IRepository<Class_TrainingUnit, int> classTrainingUnitRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _trainingProgramUnitRepository = trainingProgramUnitRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _trainingProgramSyllabusRepository = trainingProgramSyllabusRepository;
            _assessmentSchemeRepository = assessmentSchemeRepository;
            _assessmentSchemeSyllabusRepository = assessmentSchemeSyllabusRepository;
            _learningObjRepository = learningObjRepository;
            _materialsRepository = materialRepository;
            _outputStandardRepository = outputStandardRepository;
            _syllabusRepository = syllabusRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _mapper = mapper;
            _createFullSyllabusService = createFullSyllabusService;
        }

<<<<<<< ef38e5022e36f55d891e8a839b2f39209be9e309
        public async Task<(GetSyllabusByIdResponse, List<OutputStandardModel>)> getSyllabusById(int id)
=======


        //public async Task<GetSyllabusByIdModel> getSyllabusById(int id)
        //{
        //    var syllabus = await _syllabusRepository.FindByCondition(x => x.SyllabusId == id)
        //        .Include(x => x.Instructor)
        //        .Include(x => x.AssessmentScheme_Syllabus)
        //            .ThenInclude(y => y.AssessmentScheme)
        //        .Include(x => x.TrainingProgram_Syllabus)
        //            .ThenInclude(y => y.TrainingProgram)
        //        .FirstOrDefaultAsync();
        //    if (syllabus != null)
        //    {
        //        var trainingProgramUnits = await _trainingProgramUnitRepository.FindByCondition(y => y.SyllabusId == id)
        //            .ToListAsync();


        //        var    syllabusModel = new GetSyllabusByIdModel
        //        {
        //            SyllabusId = syllabus.SyllabusId,
        //            Name = syllabus.Name,
        //            Code = syllabus.Code,
        //            Description = syllabus.Description,
        //            CreatedDate = syllabus.CreatedDate,
        //            UpdatedDate = syllabus.UpdatedDate,
        //            Outline = syllabus.Outline,
        //            Level = syllabus.Level,
        //            Version = syllabus.Version,
        //            TechnicalRequirement = syllabus.TechnicalRequirement,
        //            CourseObjectives = syllabus.CourseObjectives,
        //            TrainingDelivery = syllabus.TrainingDelivery,
        //            Status = syllabus.Status,
        //            AttendeeNumber = syllabus.AttendeeNumber,
        //            InstructorId = syllabus.InstructorId,
        //            InstructorName = syllabus.Instructor?.FullName,
        //            InstructorLevel = syllabus.Instructor?.Level,
        //            TrainingProgramUnit = trainingProgramUnits
        //                .Select(unit => new TraningProgramUnitResponse
        //                {

        //                    UnitId = unit.UnitId,
        //                    UnitName = unit.UnitName,
        //                    Description = unit.Description,
        //                    Time = unit.Time,
        //                    SyllabusId = unit.SyllabusId


        //                }).ToList()
        //        };

        //        return syllabusModel;
        //    }
        //    return null;
        //}

        public async Task<GetSyllabusByIdResponse> getSyllabusById(int id)
>>>>>>> 0d7d474b5a53ec1bc24bd8ef7d0817d371501737
        {
            var syllabus = await _syllabusRepository.FindByCondition(x => x.SyllabusId == id)
                .Include(x => x.Instructor)
                .Include(x => x.AssessmentScheme_Syllabus)
                    .ThenInclude(y => y.AssessmentScheme)
                .Include(x => x.TrainingProgram_Syllabus)
                    .ThenInclude(y => y.TrainingProgram)
                .FirstOrDefaultAsync();
            
            if (syllabus != null)
            {
                var trainingProgramUnits = await _trainingProgramUnitRepository.FindByCondition(y => y.SyllabusId == id).OrderBy(x=>x.Index)
                    .ToListAsync();
                 var assessmentSchemeSyllabus = await _assessmentSchemeSyllabusRepository.FindByCondition(x=>x.SyllabusId == id).ToListAsync();
                //foreach ( var AssessmentSchemeId in assessmentSchemeSyllabus)
                //{
                //var assessmentScheme = await _assessmentSchemeRepository.FindByCondition(x => x.AssessmentSchemeId ==assessmentSchemeSyllabus.);

                //}
                var syllabusModel = new GetSyllabusByIdResponse
                {
                    SyllabusId = syllabus.SyllabusId,
                    Name = syllabus.Name,
                    Code = syllabus.Code,
                    Description = syllabus.Description,
                    CreatedDate = syllabus.CreatedDate,
                    UpdatedDate = syllabus.UpdatedDate,
                    Outline = syllabus.Outline,
                    Level = syllabus.Level,
                    Version = syllabus.Version,
                    TechnicalRequirement = syllabus.TechnicalRequirement,
                    CourseObjectives = syllabus.CourseObjectives,
                    TrainingDelivery = syllabus.TrainingDelivery,
                    Status = syllabus.Status,
                    AttendeeNumber = syllabus.AttendeeNumber,
                    InstructorId = syllabus.InstructorId,
                    InstructorName = syllabus.Instructor?.FullName,
                    Slot = trainingProgramUnits.Count(),                    
                 // InstructorLevel = syllabus.Instructor?.Level,
                    Unit = new List<TrainingProgramUnitResponse>(),
                    
                  assessmentSchemeSyllabus = new List<AssessmentSchemaResponse>()
                };
                var outPutStandard = new List<OutputStandardModel>();

                foreach (var syllabusItem in assessmentSchemeSyllabus)
                {
                    var assessmentScheme = await _assessmentSchemeRepository.FindByCondition(x => x.AssessmentSchemeId == syllabusItem.AssessmentSchemeId).FirstOrDefaultAsync();

                    if (assessmentScheme != null)
                    {
                        var assessmentSchemeModel = new AssessmentSchemaResponse
                        {
                            AssessmentSchemeId = assessmentScheme.AssessmentSchemeId,
                            SyllabusId = (int)syllabusItem.SyllabusId,
                            PercentMark = (int)syllabusItem.PercentMark,
                            AssessmentSchemeName = assessmentScheme.AssessmentSchemeName,
                        };

                        syllabusModel.assessmentSchemeSyllabus.Add(assessmentSchemeModel); // Assuming AssessmentScheme is a list
                    }
                }
                foreach (var unit in trainingProgramUnits)
                {
                    var learningObjs = await _learningObjRepository.FindByCondition(x => x.UnitId == unit.UnitId).Include(x => x.OutputStandard)
                                                                  .ToListAsync();
                    
                    var unitModel = new TrainingProgramUnitResponse
                    {
                        UnitId = unit.UnitId,
                        UnitName = unit.UnitName,
                        Description = unit.Description,
                        Time = (int)unit.Time,
                        Status=unit.Status,
                        Index = unit.Index,
                        SyllabusId = (int)unit.SyllabusId,
                        LearningObjs = new List<LearningObjResponse>()
                    };
                     outPutStandard = _mapper.Map<List<OutputStandardModel>>(learningObjs.Select(x => x.OutputStandard).DistinctBy(x => x.OutputStandardId));
                    foreach (var learningObj in learningObjs)
                    {
                        var materials = await _materialsRepository.FindByCondition(x => x.LearningObjId == learningObj.LearningObjId)
                                                                  .ToListAsync();
                        var outputStandard = await _outputStandardRepository.FindByCondition(x=>x.OutputStandardId == learningObj.OutputStandardId).FirstOrDefaultAsync();
                        var learningObjModel = new LearningObjResponse
                        {
                            UnitId = (int)learningObj.UnitId,
                            Name = learningObj.Name,
                            LearningObjId = learningObj.LearningObjId,
                            TrainningTime = (DateTime)learningObj.TrainningTime,
                            Index = (int)learningObj.Index,
                            Status = learningObj.Status,
                            DeliveryType = learningObj.DeliveryType,
                            Method = (bool)learningObj.Method,
                            OutputStandardId = learningObj.OutputStandardId,
                            Duration = learningObj.Duration,
                            OutputStandard = new OutputStandardModel
                            {
                                OutputStandardId = outputStandard.OutputStandardId,
                                Description = outputStandard.Description,
                                Tags = outputStandard.Tags,
                            },
                            


                            Material = materials.Select(material => new MaterialsResponse
                            {
                                MaterialsId = material.MaterialsId,
                                Name = material.Name,
                                CreateBy = material.CreateBy,
                                CreateDate = material.CreateDate,
                                Url = material.Url,

                            }).ToList()
                        };

                        unitModel.LearningObjs.Add(learningObjModel);
                    }


                    syllabusModel.Unit.Add(unitModel);
                }
                


                return (syllabusModel, outPutStandard);
            }

            return (null, null);
        }

        public async Task<SyllabusModel> GetSimpleSyllabusById(int id) 
        {
            var syllabus =await _syllabusRepository.GetByIdAsync(id);
            if (syllabus is null) 
            {
                throw new BadRequestException("Cannot found this Syllabus");
            }
            return _mapper.Map<SyllabusModel>(syllabus);
        }


        public async Task<(IEnumerable<SyllabusModel>, int totalPages)> GetAllSyllabusAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("PageNumber and PageSize must be positive integers.");
            }

            var totalItems = await _syllabusRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (pageNumber > totalPages)
            {
                throw new ArgumentException("PageNumber exceeds total number of pages.");
            }

            var syllabusQuery = await _syllabusRepository
                                .GetAll()
                                .Include(x => x.Instructor)
                                .Include(x => x.TrainingProgram_Syllabus)
                                .ThenInclude(x => x.TrainingProgram)
                                .OrderByDescending(x => x.CreatedDate).ToArrayAsync();

            var syllabusList = syllabusQuery
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize);

            var listSyllabusModel = new List<SyllabusModel>();
            var trainingProgramUnits = await _trainingProgramUnitRepository.GetAll().ToListAsync();

            foreach (var sy in syllabusList)
            {
                var slots = await _trainingProgramUnitRepository
                            .GetAll()
                            .Where(y => y.SyllabusId == sy.SyllabusId)
                            .Select(x => x.UnitId)
                            .ToListAsync();

                var learningObjs = _learningObjRepository.GetAll().Where(x => slots.Contains((int)x.UnitId)).Include(x => x.OutputStandard).ToList();

                var outPutStandards = _mapper.Map<List<OutputStandardModel>>(learningObjs
                                                 .Select(x => x.OutputStandard)
                                                 .DistinctBy(x => x.OutputStandardId));

                listSyllabusModel.Add(new SyllabusModel
                {
                    SyllabusId = sy.SyllabusId,
                    Name = sy.Name,
                    Code = sy.Code,
                    CreatedDate = sy.CreatedDate,
                    InStructorName = sy.Instructor?.FullName,
                    Slot = slots.Count(),
                    OutputStandards = outPutStandards.ToList(),
                    Status = sy.Status
                });

                /*var classTrainingUnits = _classTrainingUnitRepository.GetAll()
                                         .Where(x => trainningProgramIds.Contains(x.Class.ProgramId))
                                         .Include(x => x.Class)
                                         .ThenInclude(x => x.Program)
                                         .ToList();


                var listTrainingProgramUnitIds = classTrainingUnits
                                                 .Select(x => x.TrainingProgramUnitId).ToList();
                var listUnitIds = _trainingProgramUnitRepository.GetAll()
                                                 .Where(x => listTrainingProgramUnitIds.Contains(x.UnitId))
                                                 .Select(x => x.UnitId).ToList();
                var listLearningObjectIds = _learningObjRepository.GetAll()
                                                                     .Where(x => listUnitIds.Contains((int)x.UnitId))
                                                 .Include(x => x.OutputStandard).ToList();
                var outPutStandards = _mapper.Map<List<OutputStandardModel>>(listLearningObjectIds
                                                 .Select(x => x.OutputStandard)
                                                 .DistinctBy(x => x.OutputStandardId));*/


            }

            return (listSyllabusModel, totalPages);

        }
        public async Task<Entities.Syllabus> CreateSyllabus(SyllabusModel syllabusModel)
        {
            try
            {
                var syllabus = _mapper.Map<Entities.Syllabus>(syllabusModel);
                syllabus.Instructor = await _userRepository.GetByIdAsync(syllabus.InstructorId);
                var res = await _syllabusRepository.AddAsync(syllabus);
                await _syllabusRepository.Commit();
                return res;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
        public List<SyllabusModel> SearchSyllabusAsync(string keyword)
        {
            var syllabusList = _syllabusRepository
                .FindByCondition(x => x.Name != null && x.Name.Contains(keyword))
                .ToList();

            //var listSyllabusModel = _mapper.Map<List<SyllabusModel>>(syllabusList);
            var listSyllabusModel = _mapper.Map<List<SyllabusModel>>(syllabusList);
            return listSyllabusModel;
        }
        public SyllabusModel GetSyllabusByCode(string code) 
        {
            var result = _syllabusRepository.FindByCondition(s => s.Code.ToLower().Equals(code.ToLower())).FirstOrDefault();
            if (result is not null) 
            {
                return _mapper.Map<SyllabusModel>(result);
            }
            throw new BadRequestException($"Can not find this Syllabus with the code {code}");
        }

        public async Task<SyllabusModel> GetUpdateSyllabusById(int id)
        {
            var syllabus = await _syllabusRepository.GetByIdAsync(id);
            var syllabusEntity = _mapper.Map<SyllabusModel>(syllabus);
            if (syllabus is not null)
            {
                return syllabusEntity;
            }
            return null;
        }

        public async Task<UpdateSyllabusModel> UpdateSyllabus(SyllabusModel syllabusModel, CreateSyllabusModel req)
        {
            try
            {
                var syllabusEntity = _mapper.Map<Entities.Syllabus>(syllabusModel);

                var existedSyllabus = _syllabusRepository
                    .FindByCondition(s => s.SyllabusId == syllabusModel.SyllabusId && s.Status.ToLower() != "inactive")
                    .Include(s => s.Instructor)
                    .FirstOrDefault();

                if (existedSyllabus == null)
                {
                    throw new NotFoundException("Syllabus not existed!");
                }
                
                if (existedSyllabus.Status.ToLower() == "isused")
                {
                    req.SyllabusModel.Status = "Active";

                    var syllabus1 = await _createFullSyllabusService.CreateFullSyllabus(req);

                    existedSyllabus = _syllabusRepository
                        .FindByCondition(s => s.SyllabusId == syllabus1.SyllabusModel.SyllabusId)
                        .Include(s => s.Instructor)
                        .FirstOrDefault();

                    return _mapper.Map<UpdateSyllabusModel>(syllabus1);
                }

                existedSyllabus.UpdatedDate = DateTime.Now;
                if (!string.IsNullOrEmpty(req.SyllabusModel.Name))
                {
                    existedSyllabus.Name = req.SyllabusModel.Name;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.Description))
                {
                    existedSyllabus.Description = req.SyllabusModel.Description;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.Level))
                {
                    existedSyllabus.Level = req.SyllabusModel.Level;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.TechnicalRequirement))
                {
                    existedSyllabus.TechnicalRequirement = req.SyllabusModel.TechnicalRequirement;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.Outline))
                {
                    existedSyllabus.Outline = req.SyllabusModel.Outline;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.CourseObjectives))
                {
                    existedSyllabus.CourseObjectives = req.SyllabusModel.CourseObjectives;
                }
                if (!string.IsNullOrEmpty(req.SyllabusModel.TrainingDelivery))
                {
                    existedSyllabus.TrainingDelivery = req.SyllabusModel.TrainingDelivery;
                }
                if (req.SyllabusModel.AttendeeNumber > 0)
                {
                    existedSyllabus.AttendeeNumber = req.SyllabusModel.AttendeeNumber;
                }

                List<AssessmentScheme_Syllabus> list = new List<AssessmentScheme_Syllabus>();

                if (req.AssessmentSchemeSyllabusModels != null)
                {
                    bool canAdd = false;
                    req.AssessmentSchemeSyllabusModels.ForEach(a => a.SyllabusId = existedSyllabus.SyllabusId);
                    var assessmentSchemes = _mapper.Map<List<AssessmentScheme_Syllabus>>(req.AssessmentSchemeSyllabusModels);
                    assessmentSchemes.ForEach(a => a.Syllabus = existedSyllabus);
                    foreach (var assessmentScheme in assessmentSchemes)
                    {
                        var checkExisted = _assessmentSchemeSyllabusRepository.FindByCondition(a => 
                        a.AssessmentSchemeId == assessmentScheme.AssessmentSchemeId &&
                        a.SyllabusId == assessmentScheme.SyllabusId
                        ).FirstOrDefault();

                        if(checkExisted == null)
                        {
                            canAdd = true;
                        }
                        else
                        {
                            checkExisted.PercentMark = assessmentScheme.PercentMark;
                            _assessmentSchemeSyllabusRepository.Update(checkExisted);
                        }
                    }
                    if (canAdd)
                    {
                        await _assessmentSchemeSyllabusRepository.AddRangeAsync(assessmentSchemes);
                    }
                        await _assessmentSchemeSyllabusRepository.Commit();
                }

                if (req.CreateTrainingProgramUnits != null)
                {
                    foreach (var UpdateTrainingProgramUnit in req.CreateTrainingProgramUnits)
                    {
                        var checkUnitExisted = _trainingProgramUnitRepository.FindByCondition(t => t.UnitId == UpdateTrainingProgramUnit.TrainingProgramUnitModel.UnitId).FirstOrDefault();
                        if (checkUnitExisted == null)
                        {
                            UpdateTrainingProgramUnit.TrainingProgramUnitModel.SyllabusId = existedSyllabus.SyllabusId;
                            var trainingProgramUnit = _mapper.Map<TrainingProgramUnit>(UpdateTrainingProgramUnit.TrainingProgramUnitModel);
                            trainingProgramUnit.Syllabus = existedSyllabus;
                            var unit = await _trainingProgramUnitRepository.AddAsync(trainingProgramUnit);
                            await _trainingProgramUnitRepository.Commit();
                            if (UpdateTrainingProgramUnit.CreateLearningObjects != null)
                            {
                                foreach (var UpdateLearningObject in UpdateTrainingProgramUnit.CreateLearningObjects)
                                {
                                    if (UpdateLearningObject.LearningObjModel != null)
                                    {
                                        UpdateLearningObject.LearningObjModel.UnitId = unit.UnitId;
                                        var learningObject = _mapper.Map<LearningObj>(UpdateLearningObject.LearningObjModel);
                                        await _learningObjRepository.AddAsync(learningObject);
                                        await _learningObjRepository.Commit();
                                        if (UpdateLearningObject.MaterialModels != null)
                                        {
                                            UpdateLearningObject.MaterialModels.ForEach(m => m.LearningObjId = learningObject.LearningObjId);
                                            var materials = _mapper.Map<List<Materials>>(UpdateLearningObject.MaterialModels);
                                            await _materialsRepository.AddRangeAsync(materials);
                                            await _materialsRepository.Commit();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            _mapper.Map(UpdateTrainingProgramUnit.TrainingProgramUnitModel, checkUnitExisted);
                            checkUnitExisted.SyllabusId = existedSyllabus.SyllabusId;
                            var unit = _trainingProgramUnitRepository.Update(checkUnitExisted);
                            await _trainingProgramUnitRepository.Commit();
                            if (UpdateTrainingProgramUnit.CreateLearningObjects != null)
                            {
                                foreach (var UpdateLearningObject in UpdateTrainingProgramUnit.CreateLearningObjects)
                                {
                                    var checkLearningObjExisted = _learningObjRepository.FindByCondition(t => t.LearningObjId == UpdateLearningObject.LearningObjModel.LearningObjId).FirstOrDefault();
                                    if (checkLearningObjExisted == null)
                                    {
                                        UpdateLearningObject.LearningObjModel.UnitId = unit.UnitId;
                                        var learningObject = _mapper.Map<LearningObj>(UpdateLearningObject.LearningObjModel);
                                        await _learningObjRepository.AddAsync(learningObject);
                                        await _learningObjRepository.Commit();
                                        if (UpdateLearningObject.MaterialModels != null)
                                        {
                                            UpdateLearningObject.MaterialModels.ForEach(m => m.LearningObjId = learningObject.LearningObjId);
                                            var materials = _mapper.Map<List<Materials>>(UpdateLearningObject.MaterialModels);
                                            await _materialsRepository.AddRangeAsync(materials);
                                            await _materialsRepository.Commit();
                                        }
                                    }
                                    else
                                    {
                                        _mapper.Map(UpdateLearningObject.LearningObjModel, checkLearningObjExisted);
                                        _learningObjRepository.Update(checkLearningObjExisted);
                                        await _learningObjRepository.Commit();
                                        if (UpdateLearningObject.MaterialModels != null)
                                        {
                                            foreach (var UpdateMaterial in UpdateLearningObject.MaterialModels)
                                            {
                                                var checkMaterialExisted = _materialsRepository.FindByCondition(t => t.MaterialsId == UpdateMaterial.MaterialsId).FirstOrDefault();
                                                if (checkMaterialExisted == null)
                                                {
                                                    UpdateMaterial.LearningObjId = UpdateLearningObject.LearningObjModel.LearningObjId;
                                                    var material = _mapper.Map<Materials>(UpdateMaterial);
                                                    await _materialsRepository.AddAsync(material);
                                                    await _materialsRepository.Commit();
                                                }
                                                else
                                                {
                                                    _mapper.Map(UpdateMaterial, checkMaterialExisted);
                                                    _materialsRepository.Update(checkMaterialExisted);
                                                    await _materialsRepository.Commit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var syllabus = _syllabusRepository.Update(existedSyllabus);

                var result = await _syllabusRepository.Commit();

                if (result > 0)
                {
                    return _mapper.Map<UpdateSyllabusModel>(syllabus);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DuplicateSyllabus(int syllabusId)
        {
            var syllabus = await _syllabusRepository.GetByIdAsync(syllabusId);
            if (syllabus is null)
            {
                throw new BadRequestException("Cannot find this Syllabus");
            }

            var newSyllabus = new Entities.Syllabus
            {
                Name = syllabus.Name,
                Code = syllabus.Code,
                Description = syllabus.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Outline = syllabus.Outline,
                Level = syllabus.Level,
                Version = syllabus.Version,
                TechnicalRequirement = syllabus.TechnicalRequirement,
                CourseObjectives = syllabus.CourseObjectives,
                TrainingDelivery = syllabus.TrainingDelivery,
                Status = "Draft",
                AttendeeNumber = syllabus.AttendeeNumber,
                InstructorId = syllabus.InstructorId,
                Instructor = syllabus.Instructor
            };
            
            var res = await _syllabusRepository.AddAsync(newSyllabus);
            await _syllabusRepository.Commit();
            return res.SyllabusId;
        }
        public List<SyllabusModel> GetSyllabusByTrainingProgramId(int id) 
        {
            var syllabus_Program = _syllabusRepository.FindByCondition(x => x.TrainingProgram_Syllabus
            .Any(tps => tps.TrainingProgramId == id && tps.Status == "Active")).ToList();
            if (syllabus_Program == null) 
            {
                throw new BadRequestException("This syllabus does not belongs to any Training Program");
            }
            List<SyllabusModel> syllabuses = new List<SyllabusModel>();
            foreach(var syllabus in  syllabus_Program) 
            {
                var countSyllabusSlot = _trainingProgramUnitRepository.FindByCondition(tu => tu.SyllabusId == syllabus.SyllabusId).ToList().Count();
                var syllabusModel = _mapper.Map<SyllabusModel>(syllabus);
                syllabusModel.Slot = countSyllabusSlot;
                syllabuses.Add(syllabusModel);
            }
            return syllabuses;
        }

        public async Task<Entities.Syllabus> UpdateSyllabusStatus(int id, string status)
        {
            var syllabus = await _syllabusRepository.GetByIdAsync(id);
            if (syllabus is null)
            {
                throw new BadRequestException("Cannot find this Syllabus");
            }
            syllabus.Status = status;
            await _syllabusRepository.Commit();
            return syllabus;
        }

        public async Task<List<GetSyllabusByIdResponse>> getSyllabusByUserId(int id)
        {
            var listSyllabus = await _syllabusRepository.FindByCondition(x => x.InstructorId == id)
                .Include(x => x.Instructor)
                .Select(x => new GetSyllabusByIdResponse
                {
                    SyllabusId = x.SyllabusId,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                    Outline = x.Outline,
                    Level = x.Level,
                    Version = x.Version,
                    TechnicalRequirement = x.TechnicalRequirement,
                    CourseObjectives = x.CourseObjectives,
                    TrainingDelivery = x.TrainingDelivery,
                    Status = x.Status,
                    AttendeeNumber = x.AttendeeNumber,
                    InstructorId = x.InstructorId,
                    InstructorName = x.Instructor.FullName,
                    //InstructorLevel = x.Instructor.Level,
                })
                .ToListAsync();

            return listSyllabus;
        }

        public async Task<List<SyllabusModel>> getAllSyllabusByUserId(int id)
        {
            var trainingProgramId = await _userRepository.FindByCondition(x => x.UserId == id)
                .Include(x => x.TrainingProgram)
                .ThenInclude(x=>x.TrainingProgram_Syllabus)
                .ThenInclude(x=>x.Syllabus)
                .Select(x => x.TrainingProgramId)
                .FirstOrDefaultAsync();

            var listTrainingProgramId = _trainingProgramSyllabusRepository
                .GetAll()
                .Where(x => x.TrainingProgramId == trainingProgramId)
                .Select(x => x.SyllabusId).ToList();

            var listSyllabus = _syllabusRepository
                .GetAll()
                .Where(x => listTrainingProgramId.Contains(x.SyllabusId))
                .ToList();

            var listSyllabusModel = new List<SyllabusModel>();

            foreach ( var syllabus in listSyllabus) 
                {
                var getsyllabus = _syllabusRepository.FindByCondition(x=>x.SyllabusId == syllabus.SyllabusId).FirstOrDefault();
                var slots = await _trainingProgramUnitRepository
                           .GetAll()
                           .Where(y => y.SyllabusId == syllabus.SyllabusId)
                           .Select(x => x.UnitId)
                           .ToListAsync();

                var learningObjs = _learningObjRepository.GetAll().Where(x => slots.Contains((int)x.UnitId)).Include(x => x.OutputStandard).ToList();

                var outPutStandards = _mapper.Map<List<OutputStandardModel>>(learningObjs
                                                 .Select(x => x.OutputStandard)
                                                 .DistinctBy(x => x.OutputStandardId));

                listSyllabusModel.Add(new SyllabusModel
                {
                    SyllabusId = syllabus.SyllabusId,
                    Name = syllabus.Name,
                    Code = syllabus.Code,
                    CreatedDate = syllabus.CreatedDate,
                    InStructorName = syllabus.Instructor?.FullName,
                    Slot = slots.Count(),
                    OutputStandards = outPutStandards.ToList(),
                    Status = syllabus.Status
                });
            } 
            return listSyllabusModel;
         
        }



    }
}
