using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
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
    public class OutputStandardTest
    {
        private readonly IRepository<OutputStandard, int> _outputRepo;
        private readonly IMapper _mapper;
        private readonly OutputStandardService _outputStandardService;

        public OutputStandardTest()
        {

            _outputRepo = Substitute.For<IRepository<OutputStandard, int>>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OutputStandard, OutputStandardModel>();
                cfg.CreateMap<OutputStandardModel, OutputStandard>();

            }).CreateMapper();
            _outputStandardService = new OutputStandardService(_mapper, _outputRepo);

        }
        [Fact]
        public async Task ReturnsEmptyList()
        {
            var ouputStandards = new List<OutputStandard>().AsQueryable();

            // Setup mocked IRepository to return trainingPrograms
            _outputRepo.GetAll().Returns(ouputStandards);

            // Act
            var result = await _outputStandardService.GetAllOutPutStandards();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task ReturnList()
        {
            var outputStandards = new List<OutputStandard>
            {
                new OutputStandard
                {
                      OutputStandardId = 1,
                      Description = "Test",
                      Tags = "PRN"
                },
                new OutputStandard
                {
                      OutputStandardId = 2,
                      Description = "Test 1",
                      Tags = "SWP"
                },
                new OutputStandard
                {
                      OutputStandardId = 3,
                      Description = "Test 2",
                      Tags = "SWT"
                },

            };
            var test = _outputRepo.GetAll().Returns(outputStandards.AsQueryable());

            // Act
            var result = await _outputStandardService.GetAllOutPutStandards();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(outputStandards.Count());
            result.Should().OnlyContain(p => p != null);
        }
        [Fact]
        public async Task GetOutputStandardById()
        {
            var id = 1;
           
            var outputStandard = new OutputStandard
            {
                OutputStandardId = 1,
                Description = "Test",
                Tags = "PRN"
            };


            _outputRepo.GetByIdAsync(id).Returns(Task.FromResult(outputStandard));

            var result = await _outputStandardService.GetOutPutStandardById(id);

            // Assert
            result.Should().BeEquivalentTo(outputStandard);

        }
        [Fact]
        public async Task GetAssessmentSchemeById_ShouldThrow_Exception_WhenAssessmentSchemeDoesNotExist()
        {
            Func<Task> action = async () => await _outputStandardService.GetOutPutStandardById(10);

            // Assert
            await action.Should().ThrowAsync<BadHttpRequestException>();

        }
    }
}
