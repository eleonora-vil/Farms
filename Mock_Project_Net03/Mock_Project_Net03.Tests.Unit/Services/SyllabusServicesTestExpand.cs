using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
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
    public class SyllabusServiceTestExpand
    {
        private readonly SyllabusService _syllabusService;
        private readonly IRepository<User, int> _userRepository = Substitute.For<IRepository<User, int>>();
        private readonly IRepository<AssessmentScheme_Syllabus, int> _assessmentSchemeSyllabusRepository = Substitute.For<IRepository<AssessmentScheme_Syllabus, int>>();
        private readonly IRepository<TrainingProgram_Syllabus, int> _trainingProgramSyllabusRepository = Substitute.For<IRepository<TrainingProgram_Syllabus, int>>();


        private readonly IRepository<Entities.Syllabus, int> _syllabusRepository = Substitute.For<IRepository<Entities.Syllabus, int>>();
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository = Substitute.For<IRepository<Class_TrainingUnit, int>>();
        private readonly IRepository<LearningObj, int> _learningObjRepository = Substitute.For<IRepository<LearningObj, int>>(); private readonly IRepository<Materials, int> _materials = Substitute.For<IRepository<Materials, int>>();
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository = Substitute.For<IRepository<TrainingProgramUnit, int>>();
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository = Substitute.For<IRepository<TrainingProgram, int>>();
        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository = Substitute.For<IRepository<AssessmentScheme, int>>();
        private readonly IRepository<OutputStandard, int> _outputStandard = Substitute.For<IRepository<OutputStandard, int>>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        public SyllabusServiceTestExpand()
        {
            //_syllabusRepositoryMock = new Mock<IRepository<Syllabus, int>>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entities.Syllabus, SyllabusModel>().ReverseMap();
            }).CreateMapper();
            //_syllabusRepository = _syllabusRepositoryMock.Object;
            _syllabusService = new SyllabusService(
                _userRepository,
                new CreateFullSyllabusService(
                                _userRepository, // Pass the IRepository<User, int> instance here
                                _trainingProgramUnitRepository,
                                _trainingProgramRepository,
                                _assessmentSchemeRepository,
                                _learningObjRepository,
                                _materials,
                                _syllabusRepository,
                                _classTrainingUnitRepository,
                                _assessmentSchemeSyllabusRepository,
                                _mapper),
                _trainingProgramUnitRepository,
                _trainingProgramRepository,
                _trainingProgramSyllabusRepository,
                _assessmentSchemeRepository,
                _assessmentSchemeSyllabusRepository,
                _learningObjRepository,
                _materials,
                _outputStandard,
                _syllabusRepository,
                _classTrainingUnitRepository,
                _mapper);
        }


        [Fact]
        public async Task CreateSyllabus_InvalidModel_ReturnsNull()
        {
            // Arrange
            var syllabusModel = new SyllabusModel
            {
                // Set properties of the invalid syllabusModel
            };

            // Act
            var result = await _syllabusService.CreateSyllabus(syllabusModel);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SearchSyllabusAsync_KeywordExists_ReturnsMatchingSyllabi()
        {
            // Arrange
            var keyword = "sample";
            var syllabusList = new List<Entities.Syllabus>
            {
                new Entities.Syllabus
                {
                    SyllabusId = 1,
                    Name = "sample Syllabus 1",
                    Code = "SWP",
                    Description = "mini capstone",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "general",
                    Level = "Senior",
                    Version = "1.0",
                    TechnicalRequirement = "Biet Code",
                    CourseObjectives = "Biet dieu, lam viec nhom",
                    Status = "true",
                    TrainingDelivery = "sach giao khoa",
                    AttendeeNumber = 1,
                    InstructorId = 1,
                },
                new Entities.Syllabus
                {
                    SyllabusId = 2,
                    Name = "sample Syllabus 2",
                    Code = "SWP",
                    Description = "mini capstone",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "general",
                    Level = "Senior",
                    Version = "1.0",
                    TechnicalRequirement = "Biet Code",
                    CourseObjectives = "Biet dieu, lam viec nhom",
                    Status = "true",
                    TrainingDelivery = "sach giao khoa",
                    AttendeeNumber = 1,
                        InstructorId = 1,
                },
                new Entities.Syllabus
                {
                    SyllabusId = 3,
                    Name = "sample Syllabus 3",
                    Code = "SWP",
                    Description = "mini capstone",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "general",
                    Level = "Senior",
                    Version = "1.0",
                    TechnicalRequirement = "Biet Code",
                    CourseObjectives = "Biet dieu, lam viec nhom",
                    Status = "true",
                    TrainingDelivery = "sach giao khoa",
                    AttendeeNumber = 1,
                    InstructorId = 1,
                }
            };

            //var listResult = new Syllabus();
            _syllabusRepository.FindByCondition(Arg.Any<Expression<Func<Entities.Syllabus, bool>>>())
            .Returns(callInfo => syllabusList.AsQueryable());
            //    .;



            // Act
            var query = _syllabusService.SearchSyllabusAsync(keyword);
            var result = query.ToList();
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3); // Kiểm tra có 2 syllabus chứa từ khóa "sample"
            result.Select(s => s.Name).Should().Contain("sample Syllabus 1");
            result.Select(s => s.Name).Should().Contain("sample Syllabus 2");
            result.Select(s => s.Name).Should().Contain("sample Syllabus 3");

        }
    }
}

