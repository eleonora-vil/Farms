using AutoMapper;
using FluentAssertions;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using Moq;
using NSubstitute;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class UserRoleServiceTests
    {
        private readonly UserRoleService _userRoleService;
        private readonly IRepository<UserRole, int> _userRoleRepository = Substitute.For<IRepository<UserRole, int>>();
        private readonly IMapper _mapper;

        public UserRoleServiceTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRole, UserRoleModel>();
                cfg.CreateMap<UserRoleModel, UserRole>();

            }).CreateMapper();

            _userRoleService = new UserRoleService(
                _userRoleRepository,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllUserRoles()
        {
            var currentId = 1;
            var testDate = DateTime.Now;
            var UserRoleModelList = new List<UserRoleModel>();
            var UserRoleList = new List<UserRole>()
           
            {
                new UserRole{
                RoleId = 1,
                RoleName ="SuperAdmin",
                }, 
                new UserRole{ 
                    RoleId = 2,
                    RoleName = "Admin"
                }
                ,
                new UserRole{ 
                    RoleId = 3,
                    RoleName="Instructor"
                }, 
                new UserRole{ 
                    RoleId = 4,
                    RoleName="Trainee"
                }
             };
            _userRoleRepository.GetAll().Returns(UserRoleList.AsQueryable());

            foreach (var item in UserRoleList)
            {
                UserRoleModelList.Add(_mapper.Map<UserRoleModel>(item));
            }
            var result = _userRoleService.GetAll(currentId);
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task GetAllUserRoles_ReturnsNull()
        {
            var currentId = 100;
            var testDate = DateTime.Now;
            var UserRoleModelList = new List<UserRoleModel>();
            var UserRoleList = new List<UserRole>();

            //_userRoleRepository.GetAll().Returns(UserRoleList.AsQueryable());
            _userRoleRepository.GetAll().Returns(UserRoleList.AsQueryable());
            Action action = () =>
            {
                List<UserRoleModel> result = _userRoleService.GetAll(currentId);
            };

            action.Should().Throw<BadRequestException>().WithMessage("Can not find any Role");
        }



    }

}
