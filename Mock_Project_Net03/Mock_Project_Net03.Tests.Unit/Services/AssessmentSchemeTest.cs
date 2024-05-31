using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Dtos.CreateSyllabus_Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Mock_Project_Net03.Services;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net.WebSockets;
using Mock_Project_Net03.Exceptions;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class AssessmentSchemeTest
    {
                private readonly IRepository<AssessmentScheme, int> _assessmentSchemeRepo;
                private readonly IMapper _mapper;
                private readonly AssessmentSchemeService _assessmentSchemeService;

                public AssessmentSchemeTest()
                {

            _assessmentSchemeRepo = Substitute.For<IRepository<AssessmentScheme, int>>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssessmentScheme, AssessmentScheme_ToAdd>();
                cfg.CreateMap<AssessmentScheme_ToAdd, AssessmentScheme>();

            }).CreateMapper();
            _assessmentSchemeService = new AssessmentSchemeService(_mapper, _assessmentSchemeRepo);

        }
        public List<AssessmentScheme> assList = new()
            {
                new() {
                     AssessmentSchemeId = 1,
                      AssessmentSchemeName = "Quiz",
                      PercentMark = 10
                },
                new() {
                    AssessmentSchemeId = 2,
                      AssessmentSchemeName = "Final",
                      PercentMark = 20
                }
            };

        [Fact]
        public async Task GetAllAssessmentScheme_ShouldReturn_AssessmentScheme_ReturnsEmptyList()
        {
            var assessmentSchemes = new List<AssessmentScheme>().AsQueryable();

            // Setup mocked IRepository to return trainingPrograms
            _assessmentSchemeRepo.GetAll().Returns(assessmentSchemes);

            // Act
            var result = await _assessmentSchemeService.GetAssessmentSchemes();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllAssessmentScheme_ShouldReturn_AssessmentScheme_ReturnList()
        {
            var assessmentSchemes = new List<AssessmentScheme>
            {
                new AssessmentScheme
                {
                      AssessmentSchemeId = 1,
                      AssessmentSchemeName = "Quiz",
                      PercentMark = 10
                },
                new AssessmentScheme
                {
                      AssessmentSchemeId = 2,
                      AssessmentSchemeName = "Final",
                      PercentMark = 20
                },
                new AssessmentScheme
                {
                      AssessmentSchemeId = 3,
                      AssessmentSchemeName = "ProgressTest",
                      PercentMark = 30
                },

            };
            var test = _assessmentSchemeRepo.GetAll().Returns(assessmentSchemes.AsQueryable());

            // Act
            var result = await _assessmentSchemeService.GetAssessmentSchemes();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(assessmentSchemes.Count());
            result.Should().OnlyContain(p => p != null);
        }
        [Fact]
        public async Task GetAssessmentSchemeById()
        {
            var id = 1;
            var assSylla = new List<AssessmentScheme_Syllabus>
            {
                new AssessmentScheme_Syllabus {
                    AssessmentSchemeId = 1,
                    PercentMark = 10,
                    SyllabusId = id
                }
            };
            var assessmentScheme = new AssessmentScheme
            {
                AssessmentSchemeId = 1,
                AssessmentSchemeName = "Quiz",
                PercentMark = 10,
                AssessmentScheme_Syllabus = assSylla
            };

  
            _assessmentSchemeRepo.GetByIdAsync(id).Returns(Task.FromResult(assessmentScheme));

            var result = await _assessmentSchemeService.GetAssessmentSchemeById(id);

            // Assert
            result.Should().BeEquivalentTo(assessmentScheme, options => options.Excluding(x => x.AssessmentScheme_Syllabus));

        }
            [Fact]
        public async Task GetAssessmentSchemeById_ShouldThrow_Exception_WhenAssessmentSchemeDoesNotExist()
        {
            Func<Task> action = async () => await _assessmentSchemeService.GetAssessmentSchemeById(10);

            // Assert
            await action.Should().ThrowAsync<BadHttpRequestException>();

        }



    }
}
