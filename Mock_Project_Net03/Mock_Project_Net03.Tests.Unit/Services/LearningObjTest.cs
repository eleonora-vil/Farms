/*using AutoMapper;
using FluentAssertions;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services.Syllabus;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

//namespace Mock_Project_Net03.Tests.Unit.Services.Syllabus
//{
//    public class SyllabusOutlineLearningObjServicesTests
//    {
//        private readonly IRepository<LearningObj, int> _learningObjRepo;
//        private readonly IRepository<OutputStandard, int> _outputStandardRepo;
//        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepo;
//        private readonly IRepository<Materials, int> _materialRepo;
//        private readonly IMapper _mapper;
//        private readonly SyllabusOutlineLearningObjServices _syllabusOutlineLearningObjServices;

//        public SyllabusOutlineLearningObjServicesTests()
//        {
//            _learningObjRepo = Substitute.For<IRepository<LearningObj, int>>();
//            _outputStandardRepo = Substitute.For<IRepository<OutputStandard, int>>();
//            _trainingProgramUnitRepo = Substitute.For<IRepository<TrainingProgramUnit, int>>();
//            _materialRepo = Substitute.For<IRepository<Materials, int>>();
//            _mapper = Substitute.For<IMapper>();
//            _syllabusOutlineLearningObjServices = new SyllabusOutlineLearningObjServices(_learningObjRepo, _outputStandardRepo, _trainingProgramUnitRepo, _materialRepo, _mapper);
//        }

//        [Fact]
//        public async Task CreateLearningObj_WithValidData()
//        {
//            // Arrange
//            var newLearningObj = new Mock_Project_Net03.Dtos.LearningObjModel
//            {
//                Name = "Sample Learning Objective",
//                OutputStandardId = 101,
//                TrainningTime = DateTime.Now,
//                Method = true,
//                UnitId = 1
//            };

//            var outputStandardEntity = new OutputStandard
//            {
//                OutputStandardId = 101
//            };

//            var unitIdEntity = new TrainingProgramUnit
//            {
//                UnitId = 1
//            };

//            _outputStandardRepo.FindByCondition(Arg.Any<Expression<Func<OutputStandard, bool>>>())
//                .Returns(callInfo => new[] { outputStandardEntity }.AsQueryable());

//            _trainingProgramUnitRepo.FindByCondition(Arg.Any<Expression<Func<TrainingProgramUnit, bool>>>())
//                .Returns(callInfo => new[] { unitIdEntity }.AsQueryable());

//            _learningObjRepo.AddAsync(Arg.Any<LearningObj>())
//                .Returns(callInfo => Task.FromResult(callInfo.ArgAt<LearningObj>(0)));

//            _learningObjRepo.Commit().Returns(Task.FromResult(1));

//            // Act
//            var result = await _syllabusOutlineLearningObjServices.CreateLearningObj(newLearningObj);

//            // Assert
//            result.Should().BeEquivalentTo(newLearningObj);
//        }

//        [Fact]
//        public async Task CreateLearningObj_WithInvalidOutputStandardId()
//        {
//            // Arrange
//            var newLearningObj = new LearningObjModel
//            {
//                Name = "Sample Learning Objective",
//                OutputStandardId = 2,
//                TrainningTime = DateTime.Now ,
//                Method = true,
//                UnitId = 1
//            };

//            _outputStandardRepo.FindByCondition(Arg.Any<Expression<Func<OutputStandard, bool>>>())
//                .Returns(callInfo => Enumerable.Empty<OutputStandard>().AsQueryable());

//            // Act & Assert
//            await Assert.ThrowsAsync<BadRequestException>(() => _syllabusOutlineLearningObjServices.CreateLearningObj(newLearningObj));
//        }

//        [Fact]
//        public async Task CreateLearningObj_WithInvalidUnitId()
//        {
//            // Arrange
//            var newLearningObj = new LearningObjModel
//            {
//                Name = "Sample Learning",
//                OutputStandardId = 101,
//                TrainningTime = DateTime.Now,
//                Method = true,
//                UnitId = 10
//            };

//            _outputStandardRepo.FindByCondition(Arg.Any<Expression<Func<OutputStandard, bool>>>())
//                .Returns(callInfo => new[] { new OutputStandard() }.AsQueryable());

//            _trainingProgramUnitRepo.FindByCondition(Arg.Any<Expression<Func<TrainingProgramUnit, bool>>>())
//                .Returns(callInfo => Enumerable.Empty<TrainingProgramUnit>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _syllabusOutlineLearningObjServices.CreateLearningObj(newLearningObj));
        }
    }
}*/