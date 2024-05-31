﻿using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class TrainingProgramServiceTest
    {
        private readonly IRepository<TrainingProgram, int> _trainingProgramRepository;
        private readonly IMapper _mapper;
        private readonly TrainingProgramServices _trainingProgramService;
        private readonly IRepository<TrainingProgram_Syllabus, int> _trainingProgramSyllabusRepository;
        private readonly IRepository<Syllabus, int> _sy;

        public TrainingProgramServiceTest()
        {
            _trainingProgramRepository = Substitute.For<IRepository<TrainingProgram, int>>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TrainingProgram, TrainingProgramModel>();
                cfg.CreateMap<TrainingProgramModel, TrainingProgram>();

            }).CreateMapper();
            _trainingProgramSyllabusRepository = Substitute.For<IRepository<TrainingProgram_Syllabus, int>>();
            _sy = Substitute.For<IRepository<Syllabus, int>>();
            _trainingProgramService = new TrainingProgramServices(_trainingProgramRepository, _mapper, _trainingProgramSyllabusRepository, _sy);
        }

        [Fact]
        public async Task GetAllTrainingPrograms_WhenNoTrainingPrograms_ReturnsEmptyList()
        {
            // Arrange
            var trainingPrograms = new List<TrainingProgram>().AsQueryable();

            // Setup mocked IRepository to return trainingPrograms
            _trainingProgramRepository.GetAll().Returns(trainingPrograms);

            // Act
            var result = await _trainingProgramService.GetAllTrainingPrograms();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllTrainingPrograms_WhenTrainingPrograms_ReturnsTrainingPrograms()
        {
            // Arrange
            var trainingPrograms = new List<TrainingProgram>
            {
                new TrainingProgram
                {
                    ProgramId = 1,
                    ProgramName = "Training Program 1",
                    Description = "Description 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Status = "Active",
                },
                new TrainingProgram
                {
                    ProgramId = 2,
                    ProgramName = "Training Program 2",
                    Description = "Description 2",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Status = "Draft",
                },
                new TrainingProgram
                {
                    ProgramId = 3,
                    ProgramName = "Training Program 3",
                    Description = "Description 3",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Status = "Active",
                }
            };
            _trainingProgramRepository.GetAll().Returns(trainingPrograms.AsQueryable());

            // Act
            var result = await _trainingProgramService.GetAllTrainingPrograms();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(trainingPrograms.Count());
            result.Should().OnlyContain(p => p != null);
        }
        [Fact]
        public async Task UpdateTrainingProgram_WhenTrainingProgramNotFound_ThrowsBadRequestException()
        {
            // Arrange
            var trainingProgramId = 1;
            var trainingProgramModel = new TrainingProgramModel
            {
                ProgramId = 1,
                ProgramName = "Updated Training Program 1",
                Description = "Updated Description 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                CreateBy = "Namnp",
                Status = "Active",
            };

            _trainingProgramRepository.GetByIdAsync(trainingProgramId).Returns((TrainingProgram)null);

            // Act
            Func<Task> act = async () => await _trainingProgramService.UpdateTrainingProgram(trainingProgramModel);

            // Assert
            act.Should().ThrowAsync<BadRequestException>().WithMessage("Training program not found");
        }

        //[Fact]
        //public async Task UpdateTrainingProgram_WhenTrainingProgramFound_UpdatesTrainingProgram()
        //{
        //    // Arrange
        //    var trainingProgramId = 1;
        //    var syllabusId = 1;
        //    var trainingProgramModel = new TrainingProgramModel
        //    {
        //        ProgramId = 1,
        //        ProgramName = "Updated Training Program 1",
        //        Description = "Updated Description 1",
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now.AddDays(1),
        //        CreateBy = "Namnp",
        //        Status = "Active",
        //        TrainingProgram_Syllabus = new List<TrainingProgram_Syllabus>()
        //        {
        //            new TrainingProgram_Syllabus()
        //            {
        //                SyllabusId = syllabusId,
        //                TrainingProgramId = trainingProgramId,
        //            }
        //        }
//        [Fact]
//        public async Task GetAllTrainingPrograms_WhenNoTrainingPrograms_ReturnsEmptyList()
//        {
//            // Arrange
//            var trainingPrograms = new List<TrainingProgram>().AsQueryable();

//            // Setup mocked IRepository to return trainingPrograms
//            _trainingProgramRepository.GetAll().Returns(trainingPrograms);

        //    };

        //    var trainingProgram = new TrainingProgram
        //    {
        //        ProgramId = 1,
        //        ProgramName = "Training Program 1",
        //        Description = "Description 1",
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now.AddDays(1),
        //        CreateBy = "Namnp",
        //        Status = "Active",
        //        TrainingProgram_Syllabus = new List<TrainingProgram_Syllabus>()
        //        {
        //            new TrainingProgram_Syllabus()
        //            {
        //                SyllabusId = syllabusId,
        //                TrainingProgramId = trainingProgramId,
        //            }
        //        }
        //    };

        //    _trainingProgramRepository.GetByIdAsync(trainingProgramId).Returns(trainingProgram);
        //    // Act
        //    await _trainingProgramService.UpdateTrainingProgram(trainingProgramModel);

        //    // Assert
        //    _trainingProgramRepository.Received(1).Update(Arg.Is<TrainingProgram>(p =>
        //       p.ProgramId == trainingProgramId &&
        //       p.ProgramName == trainingProgramModel.ProgramName &&
        //       p.Description == trainingProgramModel.Description &&
        //       p.StartDate == trainingProgramModel.StartDate &&
        //       p.EndDate == trainingProgramModel.EndDate &&
        //       p.Status == trainingProgramModel.Status));
        //}
        [Fact]
        public async Task UpdateStatusTrainingProgram_WhenTrainingProgramNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int trainingProgramId = 1;
            string status = "Active";
            _trainingProgramRepository.GetByIdAsync(trainingProgramId).Returns((TrainingProgram)null);

            // Act
            Func<Task> act = async () => await _trainingProgramService.UpdateStatusTrainingProgram(trainingProgramId, status);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Training Program does not exist!");
        }

        //[Fact]
        //public async Task UpdateStatusTrainingProgram_WhenTrainingProgramFound_UpdatesTrainingProgramStatus()
        //{
        //    // Arrange
        //    int trainingProgramId = 1;
        //    string status = "Active";
        //    var existingTrainingProgram = new TrainingProgram
        //    {
        //        ProgramId = trainingProgramId,
        //        ProgramName = "string",
        //        Description = "string",
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now.AddDays(1),
        //        CreateBy = "Admin",
        //        Status = "Active",
        //    };

        //    _trainingProgramRepository.GetByIdAsync(trainingProgramId).Returns(existingTrainingProgram);

        //    // Act
        //    var result = await _trainingProgramService.UpdateStatusTrainingProgram(trainingProgramId, status);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.ProgramId.Should().Be(trainingProgramId);
        //    result.Status.Should().Be(status);
        //}
    }
}