using AutoMapper;
using FluentAssertions;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class SemesterServiceTest
    {
        private readonly SemesterService _semesterService;
        private readonly IRepository<Semester, int> _semesterRepo = Substitute.For<IRepository<Semester, int>>();

        public SemesterServiceTest()
        {
            _semesterService = new SemesterService(_semesterRepo);
        }

        [Fact]
        public async Task GetAllSemester_ReturnsSemester()
        {
            // Arrange
            var semesters = new List<Semester>
            {
                new Semester
                {
                    SemesterID = 1,
                    SemesterName = "Test1",
                    SemesterStartDate = DateTime.Now,
                    SemesterEndDate = DateTime.Now,
                },
                new Semester
                {
                    SemesterID = 2,
                    SemesterName = "Test2",
                    SemesterStartDate = DateTime.Now,
                    SemesterEndDate = DateTime.Now,
                },
                new Semester
                {
                    SemesterID = 2,
                    SemesterName = "Test2",
                    SemesterStartDate = DateTime.Now,
                    SemesterEndDate = DateTime.Now,
                }
            };

            _semesterRepo.GetAll().Returns(semesters.AsQueryable());

            // Act
            var result = _semesterService.GetAllSemester();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(semesters.Count());
            result.Should().OnlyContain(p => p != null);
        }

        [Fact]
        public async Task GetAllSemester_ReturnsNull()
        {
            // Arrange
            var semesters = new List<Semester>();
            _semesterRepo.GetAll().Returns(semesters.AsQueryable());

            // Act & Assert
            Action action = () =>
            {
                List<Semester> result = _semesterService.GetAllSemester();
            };

            action.Should().Throw<BadRequestException>().WithMessage("There is no semester");
        }

        [Fact]
        public async Task GetSemesterById_ReturnsSemester()
        {
            // Arrange
            var semesterId = 1;
            var semester = new Semester
            {
                SemesterID = 1,
                SemesterName = "Test1",
                SemesterStartDate = DateTime.Now,
                SemesterEndDate = DateTime.Now,
            };

            _semesterRepo.GetByIdAsync(semesterId).Returns(Task.FromResult(semester));

            // Act
            var result = await _semesterService.GetSemesterById(semesterId);

            // Assert
            result.Should().BeEquivalentTo(semester);
        }

        [Fact]
        public async Task GetSemesterById_ReturnsNull()
        {
            // Arrange
            var semesterId = 1;
            _semesterRepo.GetByIdAsync(semesterId).Returns(Task.FromResult<Semester>(null));

            // Act & Assert
            Func<Task> action = async () => await _semesterService.GetSemesterById(semesterId);

            await action.Should().ThrowAsync<BadRequestException>().WithMessage("This semester is not existed");
        }
    }
}
