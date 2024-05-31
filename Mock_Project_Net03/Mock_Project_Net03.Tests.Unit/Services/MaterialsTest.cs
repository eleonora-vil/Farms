//using System.Linq.Expressions;
//using AutoMapper;
//using Mock_Project_Net03.Dtos;
//using Mock_Project_Net03.Entities;
//using Mock_Project_Net03.Exceptions;
//using Mock_Project_Net03.Repositories;
//using Mock_Project_Net03.Services.Syllabus;
//using Newtonsoft.Json;
//using NSubstitute;
//using Xunit;

//namespace Mock_Project_Net03.Tests.Unit.Services
//{
//    public class OutlineMaterialsServicesTests
//    {
//        private readonly IRepository<Materials, int> _materialRepo;
//        private readonly IMapper _mapper;
//        private readonly IRepository<LearningObj, int> _learningObjRepo;
//        private readonly OutlineMaterialsServices _outlineMaterialsServices;

//        public OutlineMaterialsServicesTests()
//        {
//            _materialRepo = Substitute.For<IRepository<Materials, int>>();
//            _mapper = Substitute.For<IMapper>();
//            _learningObjRepo = Substitute.For<IRepository<LearningObj, int>>();
//            _outlineMaterialsServices = new OutlineMaterialsServices(_materialRepo, _mapper, _learningObjRepo);
//        }

//        [Fact]
//        public async Task CreateMaterials_WithValidData()
//        {
//            // Arrange
//            var newMaterial = new MaterialsModel
//            {
//                Name = "Sample Material",
//                CreateBy = "Nam",
//                CreateDate = DateTime.Now,
//                LearningObjId = 1
//            };

//            var learningObjEntity = new LearningObj
//            {
//                LearningObjId = 1
//            };

//            _learningObjRepo.FindByCondition(Arg.Any<Expression<Func<LearningObj, bool>>>())
//                .Returns(callInfo => new[] { learningObjEntity }.AsQueryable());

//            _materialRepo.AddAsync(Arg.Any<Materials>())
//                .Returns(callInfo => Task.FromResult(callInfo.ArgAt<Materials>(0)));

//            _materialRepo.Commit().Returns(Task.FromResult(1));

//            // Act
//            var result = await _outlineMaterialsServices.CreateMaterials(newMaterial);

//            // Assert
//            Assert.Equal(newMaterial, result);
//        }

//        [Fact]
//        public async Task CreateMaterials_WithInvalidLearningObjId()
//        {
//            // Arrange
//            var newMaterial = new MaterialsModel
//            {
//                Name = "Sample Material",
//                CreateBy = "Nam",
//                CreateDate = DateTime.Now,
//                LearningObjId = 2
//            };

//            _learningObjRepo.FindByCondition(Arg.Any<Expression<Func<LearningObj, bool>>>())
//                .Returns(callInfo => Enumerable.Empty<LearningObj>().AsQueryable());

//            // Act & Assert
//            await Assert.ThrowsAsync<BadRequestException>(() => _outlineMaterialsServices.CreateMaterials(newMaterial));
//        }
//    }
//}