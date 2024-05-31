//using AutoMapper;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using Mock_Project_Net03.Dtos;
//using Mock_Project_Net03.Entities;
//using Mock_Project_Net03.Exceptions;
//using Mock_Project_Net03.Repositories;
//using Mock_Project_Net03.Services;
//using Moq;
//using NSubstitute;
//using NSubstitute.ExceptionExtensions;
//using NSubstitute.ReceivedExtensions;
//using NSubstitute.ReturnsExtensions;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;

//namespace Mock_Project_Net03.Tests.Unit.Services
//{
//    public class SyllabusServiceTests
//    {
//        private readonly SyllabusService _syllabusService;
//        private readonly IRepository<User, int> _userRepository = Substitute.For<IRepository<User, int>>();
//        //private readonly IRepository<Syllabus, int> _syllabusRepository = Substitute.For<IRepository<Syllabus, int>>();
//        private readonly IRepository<Syllabus, int> _syllabusRepository;
//        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository = Substitute.For<IRepository<Class_TrainingUnit, int>>();
//        private readonly IRepository<LearningObj, int> _learningObjRepository = Substitute.For<IRepository<LearningObj, int>>();
//        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepository = Substitute.For<IRepository<TrainingProgramUnit, int>>();
//        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository = Substitute.For<IRepository<TrainingProgram, int>>();
//        private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepository = Substitute.For<IRepository<AssessmentScheme, int>>();
//        private readonly IMapper _mapper = Substitute.For<IMapper>();
//        public SyllabusServiceTests()
//        {
//            //_syllabusRepositoryMock = new Mock<IRepository<Syllabus, int>>();
//            _mapper = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<Syllabus, SyllabusModel>();
//            }).CreateMapper();
//            //_syllabusRepository = _syllabusRepositoryMock.Object;
//            _syllabusService = new SyllabusService(_userRepository, _syllabusRepository, _classTrainingUnitRepository, _mapper);
//        }

//        [Fact]
//        public async Task CreateSyllabusGeneral_ValidSyllabus_ReturnsSyllabus()
//        {
//            // Arrange
//            var syllabusModel = new SyllabusModel {
//                Name = "Sample Syllabus",
//                Code = "SYL-001",
//                Description = "Sample description",
//                CreatedDate = DateTime.Now,
//                UpdatedDate = DateTime.Now,
//                Outline = "Sample outline",
//                Level = "Intermediate",
//                Version = "1.0",
//                TechnicalRequirement = "Sample technical requirements",
//                CourseObjectives = "Sample course objectives",
//                TrainingDelivery = "Sample training delivery",
//                Status = "Active",
//                AttendeeNumber = 10,
//                InstructorId = 1,
//                Slot = 2,
//                InStructorName = "John Doe"
//            };
//            var syllabusEntity = new Mock_Project_Net03.Entities.Syllabus {  };

//            _mapper.Map<Entities.Syllabus>(syllabusModel).Returns(syllabusEntity);
//            _syllabusRepository.AddAsync(syllabusEntity).Returns(syllabusEntity);
//            _syllabusRepository.Commit().Returns(1);
//            // Act
//            var result = await _syllabusService.CreateSyllabus(syllabusModel);

//            // Assert
//            result.Should().BeEquivalentTo(syllabusEntity);
//        }

//        [Fact]
//        public async Task CreateSyllabusGeneral_ExceptionThrown_ReturnsBadRequest()
//        {
//            var syllabusModel = new SyllabusModel {  };

//            // Act
//            Func<Task> action = async () => await _syllabusService.CreateSyllabus(syllabusModel);

//            // Assert
//            await action.Should().ThrowAsync<BadRequestException>();
//        }
//        [Fact]
//        public async Task SearchSyllabusAsync_KeywordExists_ReturnsMatchingSyllabi()
//        {
//            // Arrange
//            var keyword = "Sample";
//            var syllabusList = new List<Syllabus>
//            {
//                new Syllabus
//                {
//                    SyllabusId = 1,
//                    Name = "sample Syllabus 1",
//                    Code = "SWP",
//                    Description = "mini capstone",
//                    CreatedDate = DateTime.Now,
//                    UpdatedDate = DateTime.Now,
//                    Outline = "general",
//                    Level = "Senior",
//                    Version = "1.0",
//                    TechnicalRequirement = "Biet Code",
//                    CourseObjectives = "Biet dieu, lam viec nhom",
//                    Status = "true",
//                    TrainingDelivery = "sach giao khoa",
//                    AttendeeNumber = 1,
//                    InstructorId = 1,
//                },
//                new Syllabus 
//                { 
//                    SyllabusId = 2,
//                    Name = "Another Syllabus",
//                    Code = "SWP",
//                    Description = "mini capstone",
//                    CreatedDate = DateTime.Now,
//                    UpdatedDate = DateTime.Now,
//                    Outline = "general",
//                    Level = "Senior",
//                    Version = "1.0",
//                    TechnicalRequirement = "Biet Code",
//                    CourseObjectives = "Biet dieu, lam viec nhom",
//                    Status = "true",
//                    TrainingDelivery = "sach giao khoa",
//                    AttendeeNumber = 1,
//                        InstructorId = 1,
//                },
//                new Syllabus 
//                { 
//                    SyllabusId = 3,
//                    Name = "sample Syllabus 2",
//                    Code = "SWP",
//                    Description = "mini capstone",
//                    CreatedDate = DateTime.Now,
//                    UpdatedDate = DateTime.Now,
//                    Outline = "general",
//                    Level = "Senior",
//                    Version = "1.0",
//                    TechnicalRequirement = "Biet Code",
//                    CourseObjectives = "Biet dieu, lam viec nhom",
//                    Status = "true",
//                    TrainingDelivery = "sach giao khoa",
//                    AttendeeNumber = 1,
//                    InstructorId = 1,
//                }
//            };

//            //var listResult = new Syllabus();
//            _syllabusRepository.FindByCondition(Arg.Any<Expression<Func<Syllabus, bool>>>())
//                .Returns( new List<Syllabus> { syllabusList.AsQueryable() });
//            //    .;



//            // Act
//            var query = _syllabusService.SearchSyllabusAsync(keyword);
//            var result = await Task.FromResult(query);
//            // Assert
//            result.Should().NotBeNull();
//            result.Should().HaveCount(2); // Kiểm tra có 2 syllabus chứa từ khóa "sample"
//            result.Select(s => s.Name).Should().Contain("sample Syllabus 1");
//            result.Select(s => s.Name).Should().Contain("sample Syllabus 2");
//        }
//    }
//}

