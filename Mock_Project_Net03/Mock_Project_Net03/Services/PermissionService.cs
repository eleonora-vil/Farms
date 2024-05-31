using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;

namespace Mock_Project_Net03.Services
{
    public class PermissionService
    {
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IRepository<Permission, int> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public Task<List<PermissionModel>> GetAllPermissions()
        {
            var result = _permissionRepository.GetAll();
            var permissionModels = _mapper.Map<List<PermissionModel>>(result);
            return Task.FromResult(permissionModels);
        }
        public PermissionModel GetPermissionByRoleId(int id)
        {
            var permission = _permissionRepository.FindByCondition(x => x.RoleID == id).FirstOrDefault();
            var permissionModel = _mapper.Map<PermissionModel>(permission);
            return permissionModel;
        }
        public async Task<PermissionModel> CreateNewPermission(PermissionModel newPermission)
        {
            var permissionEntity = _mapper.Map<Permission>(newPermission);
            var existedPermission = _permissionRepository.FindByCondition(x => x.MaterialAccess == newPermission.MaterialAccess).FirstOrDefault();
            if (existedPermission is not null)
            {
                throw new BadRequestException("permission already exist");
            }
            permissionEntity = await _permissionRepository.AddAsync(permissionEntity);
            int result = await _permissionRepository.Commit();
            if (result > 0)
            {
                newPermission.PermissionId = permissionEntity.PermissionId;
                return newPermission;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<PermissionModel>> UpdatePermissions(List<PermissionModel> updatedPermissions)
        {
            List<PermissionModel> updatedPermissionList = new List<PermissionModel>();
            HashSet<int> permissionIds = new HashSet<int>();
            HashSet<int> userRoleIds = new HashSet<int>();

            foreach (var updatedPermission in updatedPermissions)
            {
                // Kiểm tra trùng lặp PermissionId
                if (permissionIds.Contains(updatedPermission.PermissionId))
                {
                    throw new BadRequestException($"Duplicate PermissionId: {updatedPermission.PermissionId}");
                }
                // Thêm PermissionId vào HashSet
                permissionIds.Add(updatedPermission.PermissionId);

                // Kiểm tra trùng lặp UserRoleID
                if (userRoleIds.Contains(updatedPermission.RoleID))
                {
                    throw new BadRequestException($"Duplicate UserRoleId: {updatedPermission.RoleID}");
                }
                // Thêm UserRoleID vào HashSet
                userRoleIds.Add(updatedPermission.RoleID);
            }
            foreach (var updatedPermission in updatedPermissions)
            {
                List<Permission> permissions = _permissionRepository.GetAll().ToList();
                // Lấy thông tin đối tượng Permission cần cập nhật từ cơ sở dữ liệu
                var existingPermission = await _permissionRepository.GetByIdAsync(updatedPermission.PermissionId);
                
                if (existingPermission is null)
                {
                    throw new BadRequestException("Permission not found");
                }
                if (existingPermission.RoleID != updatedPermission.RoleID)
                {
                    throw new BadRequestException("This RoleID doesn't match RoleID in the database");
                }
                // Copy các trường đã cập nhật vào đối tượng Permission hiện có
                //var test = _mapper.Map(updatedPermission, existingPermission);
                existingPermission.SyllabusAccess = updatedPermission.SyllabusAccess;
                existingPermission.ProgramAccess = updatedPermission.ProgramAccess;
                existingPermission.MaterialAccess = updatedPermission.MaterialAccess;
                existingPermission.UserAccess = updatedPermission.UserAccess;
                existingPermission.ClassAccess = updatedPermission.ClassAccess;

                // Cập nhật đối tượng Permission trong cơ sở dữ liệu
                _permissionRepository.Update(existingPermission);
                updatedPermissionList.Add(updatedPermission);
            }


            // Lưu thay đổi vào cơ sở dữ liệu
            int result = await _permissionRepository.Commit();
            // Nếu cập nhật không thành công
            if (result <= 0)
            {
                return null;
            }
            return updatedPermissionList;
        }



        public async Task<PermissionModel> GetPermissionById(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            var permissionEntity = _mapper.Map<PermissionModel>(permission);
            if (permission is not null)
            {
                return permissionEntity;
            }
            return null;
        }

        public async Task<PermissionModel> GetPermissionByRoleID(int roleID) 
        {
            var permission = await _permissionRepository.FindByCondition(x => x.RoleID == roleID).FirstOrDefaultAsync();
            if (permission is not null) 
            {
                return _mapper.Map<PermissionModel>(permission);
            }
            return null;
        }
    }
}
