using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using System.Diagnostics;

namespace Mock_Project_Net03.Services
{
    public class UserRoleService
    {
        private readonly IRepository<UserRole, int> _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleService(IRepository<UserRole, int> userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }
        public async Task<UserRoleModel> GetByName(string Name) 
        {
            var userRoleEntity = _userRoleRepository.FindByCondition(x => x.RoleName.Equals(Name)).FirstOrDefault();
            if(userRoleEntity == null) 
            {
                throw new BadRequestException("Can not find this Role");
            }
            return  _mapper.Map<UserRoleModel>(userRoleEntity);
        }
        public List<UserRoleModel> GetAll(int currentId)
        {
            var roles = _userRoleRepository.FindByCondition(x => x.RoleId > currentId).ToList();
            if (roles.IsNullOrEmpty())
            {
                throw new BadRequestException("Can not find any Role");
            }
            else
            {
                return _mapper.Map<List<UserRoleModel>>(roles);
            }
        }
    }
}
