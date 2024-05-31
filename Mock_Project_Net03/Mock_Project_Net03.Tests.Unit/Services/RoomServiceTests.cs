using AutoMapper;
using CloudinaryDotNet.Actions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Mock_Project_Net03.Services.RoomService;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class RoomServiceTests
    {
        private readonly RoomService _roomService;
        private readonly IRepository<Room, int> _roomRepository = Substitute.For<IRepository<Room, int>>();
        private readonly IRepository<Class, int> _classRepository = Substitute.For<IRepository<Class, int>>();
        private readonly IRepository<Class_TrainingUnit, int> _classTrainingUnitRepository = Substitute.For<IRepository<Class_TrainingUnit, int>>();
        private readonly IRepository<Semester, int> _semesterRepository = Substitute.For<IRepository<Semester, int>>();

        public RoomServiceTests()
        {
            _roomService = new RoomService(
                _roomRepository,
                _classTrainingUnitRepository,
                _classRepository,
                _semesterRepository
                );
        }

        // Arrange
        static readonly DateTime testDate = DateTime.Now;
        public List<Room> roomLists = new()
        {
            new()
            {
                RoomId = 1,
                Name = 1,
                Description = "Test Room 1"
            },
            new()
            {
                RoomId = 2,
                Name = 2,
                Description = "Test Room 2"
            },
            new()
            {
                RoomId = 3,
                Name = 3,
                Description = "Test Room 3"
            },
            new()
            {
                RoomId = 4,
                Name = 4,
                Description = "Test Room 4"
            }
        };

        public List<Class> classList = new()
        {
            new() {
                ClassId = 1,
                ClassName = "Test1",
                SemesterId = 1,
                InstructorId = 1,
                ProgramId = 1,
                Status = "Active",
            },
            new() {
                ClassId = 2,
                ClassName = "Test2",
                SemesterId = 1,
                InstructorId = 1,
                ProgramId = 1,
                Status = "Active",
            }
        };

        public List<Semester> semesterList = new()
        {
            new()
            {
                SemesterID = 1,
                SemesterName = "SPRING 2024",
                SemesterStartDate = testDate,
                SemesterEndDate = testDate
            }
        };

        public List<Class_TrainingUnit> classTrainingUnits = new()
        {
            new()
            {
                Id = 1,
                TrainingProgramUnitId = 1,
                ClassId = 1,
                TrainerId = 1,
                RoomId = 1,
                Slot = 1,
                Day = testDate
            },
        };

        [Fact]
        public async Task GetAllRooms()
        {
            _roomRepository.GetAll().Returns(roomLists.AsQueryable());

            // act
            var result = await _roomService.GetAllRooms();
            // assert
            result.Should().BeEquivalentTo(roomLists);
        }

        [Fact]
        public async Task CheckFreeSlots()
        {
            _roomRepository.FirstOrDefault(x => x.RoomId == 1).Returns(roomLists.ElementAtOrDefault(0));
            _classTrainingUnitRepository.FindByCondition(Arg.Any<Expression<Func<Class_TrainingUnit, bool>>>()).Returns(classTrainingUnits.AsQueryable());

            //act
            var result = _roomService.CheckFreeSlots(1, testDate, testDate.AddDays(1));

            //assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task IsAvailableRoom()
        {
            _classTrainingUnitRepository.FindByCondition(Arg.Any<Expression<Func<Class_TrainingUnit, bool>>>()).Returns(classTrainingUnits.AsQueryable());

            //act
            var result = _roomService.IsAvailableRoom(1, 1, testDate);

            //assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddScheduleForClass()
        {
            _classRepository.GetByIdAsync(1).Returns(classList.ElementAtOrDefault(0));
            _semesterRepository.GetByIdAsync(1).Returns(semesterList.ElementAtOrDefault(0));
            _classTrainingUnitRepository.Commit().Returns(1); // Mocking commit to return 1

            //act
            var result = await _roomService.AddScheduleForClass(new RoomService.ScheduleInfo()
            {
                TrainingProgramUnitId = 1,
                ClassId = 1,
                TrainerId = 1,
                RoomId = 1,
                Slot = 1,
                Day = testDate,
                SemesterID = 1
            });

            //assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasSchedule()
        {
            _classTrainingUnitRepository.FindByCondition(Arg.Any<Expression<Func<Class_TrainingUnit, bool>>>()).Returns(classTrainingUnits.AsQueryable());

            // act
            var result = _roomService.HasSchedule(1);
            // assert
            result.Should().BeTrue();
        }
    }
}
