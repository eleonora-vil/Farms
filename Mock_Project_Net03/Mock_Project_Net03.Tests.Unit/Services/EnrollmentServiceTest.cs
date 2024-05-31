using AutoMapper;
using FluentAssertions;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class EnrollmentServiceTest
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly IRepository<Enrollment, int> _enrollmentRepo = Substitute.For<IRepository<Enrollment, int>>();
        private readonly IRepository<User, int> _userRepository = Substitute.For<IRepository<User, int>>();
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository = Substitute.For<IRepository<Class_TrainingUnit, int>>();
        private readonly IRepository<Class, int> _classRepository = Substitute.For<IRepository<Class, int>>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();

        public EnrollmentServiceTest()
        {
            _enrollmentService = new EnrollmentService(_enrollmentRepo, _userRepository, _classRepository, _classTrainingUnitRepository, _mapper);
            _mapper = new MapperConfiguration(cfg =>
            {

            }).CreateMapper();
        }

        [Fact]
        public async Task RemoveEnrolledStudent_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            var enrollmentId = 1;
            var enrollment = new Enrollment { /* Set up enrollment properties */ };

            _enrollmentRepo.FindByCondition(Arg.Any<Expression<Func<Enrollment, bool>>>())
                .Returns(new List<Enrollment> { enrollment }.AsQueryable());

            _enrollmentRepo.Commit().Returns(Task.FromResult(1));

            // Act
            var result = await _enrollmentService.RemoveEnrolledStudent(enrollmentId);

            // Assert
            result.Should().BeTrue();
        }

    }
}
