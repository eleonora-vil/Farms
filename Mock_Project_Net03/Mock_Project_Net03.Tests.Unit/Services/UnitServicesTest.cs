using AutoMapper;
using FluentAssertions;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services.Syllabus;
using NSubstitute;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Mock_Project_Net03.Tests.Unit.Services.Syllabus
{
    public class SyllabusOutlineUnitServicesTests
    {
        private readonly IRepository<TrainingProgramUnit, int> _trainingProgramUnitRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<Mock_Project_Net03.Entities.Syllabus, int> _syllabusRepo;
        private readonly SyllabusOutlineUnitServices _syllabusOutlineUnitServices;

        public SyllabusOutlineUnitServicesTests()
        {
            _trainingProgramUnitRepo = Substitute.For<IRepository<TrainingProgramUnit, int>>();
            _mapper = Substitute.For<IMapper>();
            _syllabusRepo = Substitute.For<IRepository<Mock_Project_Net03.Entities.Syllabus, int>>();
            _syllabusOutlineUnitServices = new SyllabusOutlineUnitServices(_trainingProgramUnitRepo, _mapper, _syllabusRepo);
        }

        [Fact]
        public async Task CreateSyllabusUnit_WithValidData()
        {
            // Arrange
            var newUnit = new TrainingProgramUnitModel
            {
                UnitName = "1",
                Description = "Sample Description",
                Time = 10,
                SyllabusId = 1
            };

            var syllabusEntity = new Mock_Project_Net03.Entities.Syllabus
            {
                SyllabusId = 1
            };

            _syllabusRepo.FindByCondition(Arg.Any<Expression<Func<Mock_Project_Net03.Entities.Syllabus, bool>>>())
                .Returns(callInfo => new[] { syllabusEntity }.AsQueryable());

            _trainingProgramUnitRepo.AddAsync(Arg.Any<TrainingProgramUnit>())
                .Returns(callInfo => Task.FromResult(callInfo.ArgAt<TrainingProgramUnit>(0)));

            _trainingProgramUnitRepo.Commit().Returns(Task.FromResult(1));

            // Act
            var result = await _syllabusOutlineUnitServices.CreateSyllabusUnit(newUnit);

            // Assert
            result.Should().BeEquivalentTo(newUnit);
        }

        [Fact]
        public async Task CreateSyllabusUnit_WithInvalidSyllabusId()
        {
            // Arrange
            var newUnit = new TrainingProgramUnitModel
            {
                UnitName = "1",
                Description = "Sample Description",
                Time = 10,
                SyllabusId = 2
            };

            _syllabusRepo.FindByCondition(Arg.Any<Expression<Func<Mock_Project_Net03.Entities.Syllabus, bool>>>())
                .Returns(callInfo => Enumerable.Empty<Mock_Project_Net03.Entities.Syllabus>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _syllabusOutlineUnitServices.CreateSyllabusUnit(newUnit));
        }
        [Fact]
        public async Task CreateSyllabusUnit_WithDuplicateUnitName()
        {
            // Arrange
            var newUnit = new TrainingProgramUnitModel
            {
                UnitName = "1",
                Description = "Sample Description",
                Time = 10,
                SyllabusId = 1
            };

            var syllabusEntity = new Mock_Project_Net03.Entities.Syllabus
            {
                SyllabusId = 1
            };

            var existingUnit = new TrainingProgramUnit
            {
                UnitName = newUnit.UnitName,
                SyllabusId = newUnit.SyllabusId
            };

            _syllabusRepo.FindByCondition(Arg.Any<Expression<Func<Mock_Project_Net03.Entities.Syllabus, bool>>>())
                .Returns(callInfo => new[] { syllabusEntity }.AsQueryable());

            _trainingProgramUnitRepo.FindByCondition(Arg.Any<Expression<Func<TrainingProgramUnit, bool>>>())
                .Returns(callInfo => new[] { existingUnit }.AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _syllabusOutlineUnitServices.CreateSyllabusUnit(newUnit));
        }
    }
}