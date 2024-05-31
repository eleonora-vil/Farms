using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class ClassServiceTests
    {/*
        private readonly ClassService _classService;
        private readonly IRepository<Class, int> _classRepository = Substitute.For<IRepository<Class, int>>();
        private readonly IRepository<Attendance, int> _attendanceRepository = Substitute.For<IRepository<Attendance, int>>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        public ClassServiceTests()
        {
            _classService = new ClassService(_classRepository, _attendanceRepository, _mapper);
        }

        [Fact]
        public async Task GetAllClass()
        {
            // Arrange
            List<Class> classList = new List<Class>();
            List<ClassModel> classModelList = new List<ClassModel>();
            (IEnumerable<ClassModel>, int) result = (classModelList, 0);

            var totalItems = await _classRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / 10);

            _classRepository
               .GetAll()
               .Include(x => x.Instructor)
               .Include(x => x.Program)
               .Skip((1 - 1) * 10)
               .Take(10)
               .Returns(classList.AsQueryable());

            _mapper.Map<List<ClassModel>>(classList)
                .Returns(classModelList);

            // Act
            _classService.GetAllClass(1, 10).Returns(result);

            // Assert
            result.Should().BeEquivalentTo((classModelList, totalPages));
        }

        [Fact]
        public async Task GetAllClassWithFilter_KeyWord()
        {
            // Arrange
            var (classesList, totalPages) = await _classService.GetAllClass(1, 10);
            var classes = classesList;

            classes = classes
                .Where(x => x.ClassName.ToLower().Contains("neer")); // class name match keyword

            // Act
            var result = _classService.GetAllClassWithFilter(classesList, "neer", "", "", "", DateTime.Parse("2024 - 03 - 18 16:39:21.881756"), DateTime.Parse("2024 - 03 - 29 16:39:21.881756"), "", 0);

            // Assert
            result.Should().BeEquivalentTo(classes);
        }

        [Fact]
        public async Task GetAllClassWithFilter_ReturnNull()
        {
            // Arrange
            var (classesList, totalPages) = await _classService.GetAllClass(1, 10);

            // Act
            var result = _classService.GetAllClassWithFilter(classesList, "expected to be empty", "", "", "", DateTime.Parse("2024 - 03 - 20 16:39:21.881756"), DateTime.Parse("2024 - 03 - 29 16:39:21.881756"), "", 0);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetClassById()
        {
            // Arrange
            var classEntity = new Class { };
            var classModel = new ClassModel { };

            _classRepository
                .FindByCondition(x => x.ClassId == 1)
                .Include(x => x.Instructor)
                .Include(x => x.Program)
                .FirstOrDefaultAsync()
                .Returns(classEntity);

            _mapper.Map<ClassModel>(classEntity).Returns(classModel);

            // Act
            var result = await _classService.GetClassById(1);

            // Assert
            result.Should().BeEquivalentTo(classModel);
        }

        [Fact]
        public async Task GetClassById_ReturnNull()
        {
            // arrange
            var result = new ClassModel();
            
            // Act
            _classService.GetClassById(10)
                .Returns(Task.FromResult(result));

            // Assert
            result.Should().Throws<NotFoundException>();
        }

        [Fact]
        public async Task GetClassDetail()
        {
            // Arrange
            GetClassDetailResponse response = new GetClassDetailResponse();
            Class classEntity = new Class { };
            List<Attendance> attendees = new List<Attendance>();

            _classRepository
                .FindByCondition(x => x.ClassId == 1)
                .Include(x => x.Instructor)
                .Include(x => x.Program)
                .FirstOrDefaultAsync()
                .Returns(classEntity);

            _attendanceRepository
                .GetAll()
                .Where(x => x.ClassId == classEntity.ClassId)
                .ToListAsync().Returns(attendees);

            response.Class = _mapper.Map<ClassModel>(classEntity);
            response.attendees = attendees;
            // Act
            var result = await _classService.GetClassDetail(1);

            // Assert
            result.Should().BeEquivalentTo(response);
        }*/
    }
}
