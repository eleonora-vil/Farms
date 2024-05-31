using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Services.Syllabus;
using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class SyllabusServiceTests
    {
        private readonly SyllabusService? _syllabusService;
        private readonly IRepository<User, int> _userRepository = Substitute.For<IRepository<User, int>>();
        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository = Substitute.For<IRepository<Entities.Syllabus, int>>();
        private readonly CreateFullSyllabusService _createFullSyllabusService;
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository = Substitute.For<IRepository<Class_TrainingUnit, int>>();
        private readonly IRepository<LearningObj, int> _learningObjRepository = Substitute.For<IRepository<LearningObj, int>>();
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository = Substitute.For<IRepository<TrainingProgramUnit, int>>();
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository = Substitute.For<IRepository<TrainingProgram, int>>();
        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository = Substitute.For<IRepository<AssessmentScheme, int>>();
        private readonly IRepository<AssessmentScheme_Syllabus, int> _assessmentSchemeSyllabusRepository = Substitute.For<IRepository<AssessmentScheme_Syllabus, int>>();
        private readonly IRepository<OutputStandard, int> _outputStandardRepository = Substitute.For<IRepository<OutputStandard, int>>();
        private readonly IRepository<Materials, int> _materialsRepository = Substitute.For<IRepository<Materials, int>>();
        private readonly IRepository<TrainingProgram_Syllabus, int> _trainingProgramSyllabusRepository = Substitute.For<IRepository<TrainingProgram_Syllabus, int>>();
        private readonly IMapper _mapper;
        public SyllabusServiceTests()
        {
            _mapper = Substitute.For<IMapper>();
            _createFullSyllabusService = new CreateFullSyllabusService(_userRepository, _trainingProgramUnitRepository, _trainingProgramRepository,
                _assessmentSchemeRepository, _learningObjRepository, _materialsRepository, _syllabusRepository, _classTrainingUnitRepository,
                _assessmentSchemeSyllabusRepository, _mapper);

            _syllabusService = new SyllabusService(_userRepository, _createFullSyllabusService, _trainingProgramUnitRepository,
         _trainingProgramRepository, _trainingProgramSyllabusRepository, _assessmentSchemeRepository, _assessmentSchemeSyllabusRepository, _learningObjRepository,
         _materialsRepository, _outputStandardRepository, _syllabusRepository, _classTrainingUnitRepository, _mapper);
        }

        [Fact]
        public async Task UpdateSyllabus_WithValidInputs_ReturnsUpdatedSyllabus()
        {
            // Arrange
            var syllabusUpdate = new Syllabus_CreateSyllabusModel
            {
                SyllabusId = 1,
                Name = "Updated Syllabus",
                Description = "This is an updated syllabus.",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Code = "UPD-001",
                Outline = "This is the syllabus outline.",
                Level = "Beginner",
                Version = "1.1",
                TechnicalRequirement = "Students must have a computer.",
                CourseObjectives = "Students will learn about XYZ.",
                TrainingDelivery = "In-person",
                Status = "Active",
                AttendeeNumber = 15,
                InstructorId = 2,
                Slot = 3
            };

            var existingSyllabus = new Entities.Syllabus
            {
                SyllabusId = 1,
                InstructorId = 1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Name = "Sample Syllabus",
                Code = "S001",
                Description = "This is a sample syllabus",
                Outline = "Sample syllabus outline",
                Level = "Beginner",
                Version = "1.0",
                TechnicalRequirement = "Students should have access to a computer",
                CourseObjectives = "Students will learn the basics of programming",
                TrainingDelivery = "In-person",
                Status = "Active",
                AttendeeNumber = 15
            };

            var existingSyllabusModel = new SyllabusModel
            {
                SyllabusId = 1,
            };

            var updatedSyllabus = new Entities.Syllabus
            {
                Name = "Updated Syllabus",
                Description = "This is an updated syllabus.",
                AssessmentScheme_Syllabus = null,
                TrainingProgram_Syllabus = null,
                Instructor = null,
                Level = "Beginner",
                Version = "1.1",
                TechnicalRequirement = "Students must have a computer.",
                CourseObjectives = "Students will learn about XYZ.",
                TrainingDelivery = "In-person",
                Status = "Active",
                AttendeeNumber = 15,
                InstructorId = 2
            };

            var materialModel = new MaterialModel
            {
                MaterialsId = 1,
                Name = "Test",
                CreateDate = DateTime.Now,
                LearningObjId = 1,
                Url = "Test"
            };

            var existingMaterial = new Materials
            {
                MaterialsId = 1,
                Name = "Test",
                LearningObjId = 1,
                Url = "Test"
            };

            var updatedMaterial = new Materials
            {
                MaterialsId = 1,
                Name = "Test",
                LearningObjId = 1,
                Url = "Test"
            };

            var listMaterialModel = new List<MaterialModel>
            {
                materialModel
            };

            var learningObjModel = new LearningObj_CreateSyllabusModel
            {
                LearningObjId = 1,
                Status = "Active",
                Name = "Test",
                UnitId = 1,
                OutputStandardId = 1
            };

            var existingLearningObj = new LearningObj
            {
                LearningObjId = 1,
                Status = "Active",
                Name = "Existing",
                UnitId = 1,
                OutputStandardId = 1
            };

            var updatedLearningObj = new LearningObj
            {
                LearningObjId = 1,
                Status = "Active",
                Name = "Test",
                UnitId = 1,
                OutputStandardId = 1
            };

            var createLearningObjModel = new List<CreateLearningObject>
            {
                new CreateLearningObject
                {
                    LearningObjModel = learningObjModel,
                    MaterialModels = listMaterialModel
                }
            };

            var trainingUnitModel = new TrainingProgramUnit_CreateSyllabusModel
            {
                UnitId = 1,
                UnitName = "Test",
                SyllabusId = 1,
                Status = "Active",
                Description = "Test"
            };

            var existingTrainingUnitModel = new TrainingProgramUnit
            {
                UnitId = 1,
                UnitName = "Existing",
                SyllabusId = 1,
                Status = "Active",
                Description = "Existing"
            };

            var updatedTrainingUnitModel = new TrainingProgramUnit
            {
                UnitId = 1,
                UnitName = "Test",
                SyllabusId = 1,
                Status = "Active",
                Description = "Test"
            };

            var createTrainingProgramUnitModel = new List<CreateTrainingProgramUnit>
            {
                new CreateTrainingProgramUnit
                {
                    CreateLearningObjects = createLearningObjModel,
                    TrainingProgramUnitModel = trainingUnitModel
                }
            };

            var assessmentSchemeModel = new AssessmentScheme_CreateSyllabusModel
            {
                AssessmentSchemeId = 1,
                PercentMark = 50,
                SyllabusId = 1
            };

            var existingAssessmentScheme = new AssessmentScheme_Syllabus
            {
                AssessmentSchemeId = 1,
                PercentMark = 40,
                SyllabusId = 1
            };

            var updateAssessmentScheme = new AssessmentScheme_Syllabus
            {
                AssessmentSchemeId = 1,
                PercentMark = 50,
                SyllabusId = 1
            };

            var listAssessmentSchemeModel = new List<AssessmentScheme_CreateSyllabusModel>
            {
                assessmentSchemeModel
            };

            var createSyllabusModel = new CreateSyllabusModel
            {
                SyllabusModel = syllabusUpdate,
                CreateTrainingProgramUnits = createTrainingProgramUnitModel,
                AssessmentSchemeSyllabusModels = listAssessmentSchemeModel
            };

            //var existingSyllabusModel = _mapper.Map<SyllabusModel>(existingSyllabus);

            _mapper.Map<List<AssessmentScheme_Syllabus>>(Arg.Any<List<AssessmentScheme_CreateSyllabusModel>>()).Returns(createSyllabusModel.AssessmentSchemeSyllabusModels.Select(assessment => new AssessmentScheme_Syllabus
            {
                AssessmentSchemeId = assessment.AssessmentSchemeId,
                SyllabusId = assessment.SyllabusId,
                PercentMark = assessment.PercentMark
            }).ToList());

            _mapper.Map<TrainingProgramUnit>(Arg.Any<TrainingProgramUnit_CreateSyllabusModel>())
                .Returns(new TrainingProgramUnit
                {
                    UnitName = trainingUnitModel.UnitName,
                    SyllabusId = trainingUnitModel.SyllabusId,
                    Description = trainingUnitModel.Description,
                    Time = trainingUnitModel.Time,
                    Status = trainingUnitModel.Status,
                    Index = trainingUnitModel.Index
                });

            _mapper.Map<LearningObj>(Arg.Any<LearningObj_CreateSyllabusModel>())
                .Returns(new LearningObj
                {
                    Name = learningObjModel.Name,
                    OutputStandardId = learningObjModel.OutputStandardId,
                    TrainningTime = learningObjModel.TrainningTime,
                    Method = learningObjModel.Method,
                    UnitId = learningObjModel.UnitId
                });

            _mapper.Map<Materials>(Arg.Any<MaterialModel>())
                 .Returns(new Materials
                 {
                     MaterialsId = materialModel.MaterialsId,
                     Name = materialModel.Name,
                     CreateDate = materialModel.CreateDate,
                     Url = materialModel.Url,
                     LearningObjId = materialModel.LearningObjId
                 });

            _mapper.Map<UpdateSyllabusModel>(Arg.Any<Entities.Syllabus>())
                            .Returns(new UpdateSyllabusModel
                            {
                                Name = updatedSyllabus.Name,
                                Description = updatedSyllabus.Description,
                                Level = updatedSyllabus.Level,
                                Version = updatedSyllabus.Version,
                                TechnicalRequirement = updatedSyllabus.TechnicalRequirement,
                                CourseObjectives = updatedSyllabus.CourseObjectives,
                                TrainingDelivery = updatedSyllabus.TrainingDelivery,
                                Status = updatedSyllabus.Status,
                                AttendeeNumber = (int)updatedSyllabus.AttendeeNumber,
                                InstructorId = updatedSyllabus.InstructorId
                            });

            // Assert
            _mapper.Map<Entities.Syllabus>(syllabusUpdate).Returns(existingSyllabus);
            _syllabusRepository.FindByCondition(Arg.Any<Expression<Func<Entities.Syllabus, bool>>>())
                               .Returns(new List<Entities.Syllabus> { existingSyllabus }.AsQueryable());
            _syllabusRepository.Update(Arg.Any<Entities.Syllabus>()).Returns(updatedSyllabus);

            _mapper.Map<TrainingProgramUnit>(trainingUnitModel).Returns(existingTrainingUnitModel);
            _trainingProgramUnitRepository.FindByCondition(Arg.Any<Expression<Func<TrainingProgramUnit, bool>>>())
                   .Returns(new List<TrainingProgramUnit> { existingTrainingUnitModel }.AsQueryable());
            _trainingProgramUnitRepository.Update(Arg.Any<TrainingProgramUnit>()).Returns(updatedTrainingUnitModel);

            _mapper.Map<LearningObj>(learningObjModel).Returns(existingLearningObj);
            _learningObjRepository.FindByCondition(Arg.Any<Expression<Func<LearningObj, bool>>>())
                   .Returns(new List<LearningObj> { existingLearningObj }.AsQueryable());
            _learningObjRepository.Update(Arg.Any<LearningObj>()).Returns(updatedLearningObj);

            _mapper.Map<Materials>(materialModel).Returns(existingMaterial);
            _materialsRepository.FindByCondition(Arg.Any<Expression<Func<Materials, bool>>>())
                   .Returns(new List<Materials> { existingMaterial }.AsQueryable());
            _materialsRepository.Update(Arg.Any<Materials>()).Returns(updatedMaterial);

            _mapper.Map<AssessmentScheme_Syllabus>(assessmentSchemeModel).Returns(existingAssessmentScheme);
            _assessmentSchemeSyllabusRepository.FindByCondition(Arg.Any<Expression<Func<AssessmentScheme_Syllabus, bool>>>())
                   .Returns(new List<AssessmentScheme_Syllabus> { existingAssessmentScheme }.AsQueryable());
            _assessmentSchemeSyllabusRepository.Update(Arg.Any<AssessmentScheme_Syllabus>()).Returns(updateAssessmentScheme);
            _syllabusRepository.Commit().Returns(1);

            var expect = _mapper.Map<UpdateSyllabusModel>(updatedSyllabus);

            // Act
            var result = await _syllabusService.UpdateSyllabus(existingSyllabusModel, createSyllabusModel);

            result.Should().NotBeNull();
        }
    }
}

