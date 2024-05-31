using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Transactions;

namespace Mock_Project_Net03.Services.Syllabus
{
    public class CreateFullSyllabusService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository;
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository;
        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository;
        private readonly IRepository<AssessmentScheme_Syllabus, int> _assessmentSchemeSyllabusRepository;

        private readonly IRepository<LearningObj, int> _learningObjRepository;
        private readonly IRepository<Materials, int> _materialsRepository;
        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository;
        private readonly IMapper _mapper;
        private IRepository<Entities.Syllabus, int> syllabusRepository;
        private IMapper mapper;

        public CreateFullSyllabusService(IRepository<User, int> userRepository,
            IRepository<TrainingProgramUnit, int> trainingProgramUnitRepository,
            IRepository<TrainingProgram, int> trainingProgramRepository,
            IRepository<AssessmentScheme, int> assessmentSchemeRepository,
            IRepository<LearningObj, int> learningObjRepository,
            IRepository<Materials, int> materialRepository,
            IRepository<Entities.Syllabus, int> syllabusRepository,
            IRepository<Class_TrainingUnit, int> classTrainingUnitRepository,
            IRepository<AssessmentScheme_Syllabus, int> assessmentSchemeSyllabusRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _trainingProgramUnitRepository = trainingProgramUnitRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _assessmentSchemeRepository = assessmentSchemeRepository;
            _learningObjRepository = learningObjRepository;
            _materialsRepository = materialRepository;
            _syllabusRepository = syllabusRepository;
            _classTrainingUnitRepository = classTrainingUnitRepository;
            _assessmentSchemeSyllabusRepository = assessmentSchemeSyllabusRepository;
            _mapper = mapper;
        }
        public async Task<CreateSyllabusModel> CreateFullSyllabus(CreateSyllabusModel createSyllabusModel)
        {
            
            bool isValid = true;
            CreateSyllabusModel result = null;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (createSyllabusModel.SyllabusModel.Status.ToLower().Equals("draft"))
                    {
                        var syllabusDraft = _mapper.Map<Entities.Syllabus>(createSyllabusModel.SyllabusModel);
                        await _syllabusRepository.AddAsync(syllabusDraft);
                        await _syllabusRepository.Commit();

                        createSyllabusModel.AssessmentSchemeSyllabusModels.ForEach(a => a.SyllabusId = syllabusDraft.SyllabusId);
                        var assessmentSchemes = _mapper.Map<List<AssessmentScheme_Syllabus>>(createSyllabusModel.AssessmentSchemeSyllabusModels);
                        await _assessmentSchemeSyllabusRepository.AddRangeAsync(assessmentSchemes);
                        await _assessmentSchemeSyllabusRepository.Commit();
                        foreach (var CreateTrainingProgramUnit in createSyllabusModel.CreateTrainingProgramUnits)
                        {

                            CreateTrainingProgramUnit.TrainingProgramUnitModel.SyllabusId = syllabusDraft.SyllabusId;
                            var trainingProgramUnit = _mapper.Map<TrainingProgramUnit>(CreateTrainingProgramUnit.TrainingProgramUnitModel);
                            var unit = await _trainingProgramUnitRepository.AddAsync(trainingProgramUnit);
                            await _trainingProgramUnitRepository.Commit();

                            foreach (var CreateLearningObject in CreateTrainingProgramUnit.CreateLearningObjects)
                            {
                                CreateLearningObject.LearningObjModel.UnitId = unit.UnitId;

                                var learningObject = _mapper.Map<LearningObj>(CreateLearningObject.LearningObjModel);
                                await _learningObjRepository.AddAsync(learningObject);
                                await _learningObjRepository.Commit();
                                if (CreateLearningObject.MaterialModels != null)
                                {
                                    CreateLearningObject.MaterialModels.ForEach(m => m.LearningObjId = learningObject.LearningObjId);
                                    var materials = _mapper.Map<List<Materials>>(CreateLearningObject.MaterialModels);
                                    await _materialsRepository.AddRangeAsync(materials);
                                    await _materialsRepository.Commit();
                                }
                            }
                        }
                        result = createSyllabusModel;
                        scope.Complete();
                    }
                    else if (createSyllabusModel.SyllabusModel.Status.ToLower().Equals("active"))
                    {
                        if (createSyllabusModel == null)
                        {
                            return result;
                        }
                        if (createSyllabusModel.SyllabusModel == null)
                        {
                            return result;
                        }
                        if (createSyllabusModel.CreateTrainingProgramUnits == null)
                        {
                            return result;
                        }
                        if (createSyllabusModel.AssessmentSchemeSyllabusModels == null)
                        {
                            return result;
                        }
                        if (createSyllabusModel.SyllabusModel.Name == null
                            || createSyllabusModel.SyllabusModel.Code == null
                            || createSyllabusModel.SyllabusModel.Description == null
                            || createSyllabusModel.SyllabusModel.Outline == null
                            || createSyllabusModel.SyllabusModel.Level == null
                            || createSyllabusModel.SyllabusModel.Version == null
                            || createSyllabusModel.SyllabusModel.TechnicalRequirement == null
                            || createSyllabusModel.SyllabusModel.CourseObjectives == null
                            || createSyllabusModel.SyllabusModel.TrainingDelivery == null
                            || createSyllabusModel.SyllabusModel.Status == null
                            || createSyllabusModel.SyllabusModel.AttendeeNumber == null
                            || createSyllabusModel.SyllabusModel.InstructorId == null
                            || createSyllabusModel.SyllabusModel.Slot == null)
                        {
                            throw new BadRequestException("No fields can be null");
                        }
                        foreach (var assessmentSchemeModel in createSyllabusModel.AssessmentSchemeSyllabusModels)
                        {
                            if (assessmentSchemeModel.AssessmentSchemeId == null
                                || assessmentSchemeModel.PercentMark == null)
                            {
                                throw new BadRequestException("No fields can be null");

                            }
                        }
                        foreach (var trainingProgramUnit in createSyllabusModel.CreateTrainingProgramUnits)
                        {
                            if (trainingProgramUnit.TrainingProgramUnitModel.UnitName == null
                                || trainingProgramUnit.TrainingProgramUnitModel.Description == null
                                || trainingProgramUnit.TrainingProgramUnitModel.Time == null
                                || trainingProgramUnit.TrainingProgramUnitModel.Status == null
                                || trainingProgramUnit.TrainingProgramUnitModel.Index == null)
                            {
                                throw new BadRequestException("No fields can be null");

                            }
                        }
                        foreach (var learningObject in createSyllabusModel.CreateTrainingProgramUnits.SelectMany(x => x.CreateLearningObjects))
                        {
                            if (learningObject.LearningObjModel.Name == null
                                || learningObject.LearningObjModel.TrainningTime == null
                                || learningObject.LearningObjModel.Method == null
                                || learningObject.LearningObjModel.Index == null
                                || learningObject.LearningObjModel.Status == null
                                || learningObject.LearningObjModel.DeliveryType == null
                                || learningObject.LearningObjModel.OutputStandardId == null
                                || learningObject.LearningObjModel.Duration == null)
                            {
                                throw new BadRequestException("No fields can be null");

                            }
                        }
                        createSyllabusModel.SyllabusModel.CreatedDate = DateTime.Now;
                        var syllabus = _mapper.Map<Entities.Syllabus>(createSyllabusModel.SyllabusModel);
                        await _syllabusRepository.AddAsync(syllabus);
                        await _syllabusRepository.Commit();
                        createSyllabusModel.SyllabusModel.SyllabusId = syllabus.SyllabusId;

                        List<AssessmentScheme_Syllabus> list = new List<AssessmentScheme_Syllabus>();

                        if (createSyllabusModel.AssessmentSchemeSyllabusModels != null)
                        {
                            createSyllabusModel.AssessmentSchemeSyllabusModels.ForEach(a => a.SyllabusId = syllabus.SyllabusId);
                            var assessmentSchemes = _mapper.Map<List<AssessmentScheme_Syllabus>>(createSyllabusModel.AssessmentSchemeSyllabusModels);
                            await _assessmentSchemeSyllabusRepository.AddRangeAsync(assessmentSchemes);
                            await _assessmentSchemeSyllabusRepository.Commit();
                        }
                        else
                        {
                            isValid = false;
                        }

                        if (createSyllabusModel.CreateTrainingProgramUnits != null)
                        {
                            foreach (var CreateTrainingProgramUnit in createSyllabusModel.CreateTrainingProgramUnits)
                            {
                                if (CreateTrainingProgramUnit.TrainingProgramUnitModel != null)
                                {
                                    CreateTrainingProgramUnit.TrainingProgramUnitModel.SyllabusId = syllabus.SyllabusId;
                                    var trainingProgramUnit = _mapper.Map<TrainingProgramUnit>(CreateTrainingProgramUnit.TrainingProgramUnitModel);
                                    var unit = await _trainingProgramUnitRepository.AddAsync(trainingProgramUnit);
                                    await _trainingProgramUnitRepository.Commit();
                                    if (CreateTrainingProgramUnit.CreateLearningObjects != null)
                                    {
                                        foreach (var CreateLearningObject in CreateTrainingProgramUnit.CreateLearningObjects)
                                        {

                                            if (CreateLearningObject.LearningObjModel != null)
                                            {
                                                CreateLearningObject.LearningObjModel.UnitId = unit.UnitId;
                                                var learningObject = _mapper.Map<LearningObj>(CreateLearningObject.LearningObjModel);
                                                await _learningObjRepository.AddAsync(learningObject);
                                                await _learningObjRepository.Commit();
                                                if (CreateLearningObject.MaterialModels != null)
                                                {
                                                    CreateLearningObject.MaterialModels.ForEach(m => m.LearningObjId = learningObject.LearningObjId);
                                                    var materials = _mapper.Map<List<Materials>>(CreateLearningObject.MaterialModels);
                                                    await _materialsRepository.AddRangeAsync(materials);
                                                    await _materialsRepository.Commit();
                                                }

                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }

                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                        }
                        else
                        {
                            isValid = false;
                        }
                        if (isValid)
                        {
                            result = createSyllabusModel;
                            scope.Complete();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {

                    throw new Exception("Có lỗi xảy ra trong quá trình xử lý", ex);
                
            }
            }
            return result;
        }
    /*    public async Task<CreateSyllabusModel> CreateFullSyllabusDraft(CreateSyllabusModel createSyllabusModel)
        {
            
            CreateSyllabusModel result = null;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                  
                    createSyllabusModel.SyllabusModel.Status = "Draft";
                    var syllabus = _mapper.Map<Entities.Syllabus>(createSyllabusModel.SyllabusModel);
                    await _syllabusRepository.AddAsync(syllabus);
                    await _syllabusRepository.Commit();

                    List<AssessmentScheme_Syllabus> list = new List<AssessmentScheme_Syllabus>();

                        createSyllabusModel.AssessmentSchemeSyllabusModels.ForEach(a => a.SyllabusId = syllabus.SyllabusId);
                        var assessmentSchemes = _mapper.Map<List<AssessmentScheme_Syllabus>>(createSyllabusModel.AssessmentSchemeSyllabusModels);
                        await _assessmentSchemeSyllabusRepository.AddRangeAsync(assessmentSchemes);
                        await _assessmentSchemeSyllabusRepository.Commit();
                  

                  
                        foreach (var CreateTrainingProgramUnit in createSyllabusModel.CreateTrainingProgramUnits)
                        {
                          
                                CreateTrainingProgramUnit.TrainingProgramUnitModel.SyllabusId = syllabus.SyllabusId;
                                var trainingProgramUnit = _mapper.Map<TrainingProgramUnit>(CreateTrainingProgramUnit.TrainingProgramUnitModel);
                                var unit = await _trainingProgramUnitRepository.AddAsync(trainingProgramUnit);
                                await _trainingProgramUnitRepository.Commit();
                               
                                    foreach (var CreateLearningObject in CreateTrainingProgramUnit.CreateLearningObjects)
                                    {                                      
                                            CreateLearningObject.LearningObjModel.UnitId = unit.UnitId;

                                            CreateLearningObject.LearningObjModel.OutputStandardId = CreateLearningObject.OutputStandardModel.OutputStandardId;
                                          
                                                var learningObject = _mapper.Map<LearningObj>(CreateLearningObject.LearningObjModel);
                                                await _learningObjRepository.AddAsync(learningObject);
                                                await _learningObjRepository.Commit();
                                                CreateLearningObject.MaterialModels.ForEach(m => m.LearningObjId = learningObject.LearningObjId);
                                                var materials = _mapper.Map<List<Materials>>(CreateLearningObject.MaterialModels);
                                                await _materialsRepository.AddRangeAsync(materials);                                                                               
                                    }
                                }
                                result = createSyllabusModel;
                                scope.Complete();
                }                                                                                                      
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra trong quá trình xử lý", ex);

                }
            }
            return result;
        }
     */
    }
}
