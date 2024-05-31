using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;
using Xunit;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class PermissionServiceUnitTests
    {
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IMapper _mapper;
        private readonly PermissionService _permissionService;

        public PermissionServiceUnitTests()
        {
            // Create mock for IRepository and IMapper
            _permissionRepository = Substitute.For<IRepository<Permission, int>>();
            // Khởi tạo và cấu hình AutoMapper


            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Permission, PermissionModel>();
            }).CreateMapper();


            // Initialize PermissionService with mocked IRepository and IMapper
            _permissionService = new PermissionService(_permissionRepository, _mapper);
        }

        [Fact]
        public async Task UpdatePermissions_WithValidPermissions_ReturnsUpdatedPermissions()
        {
            // Arrange
            // Create fake data for PermissionModel
            var permissionModels = new List<PermissionModel>
            {
                new PermissionModel
                {
                    PermissionId = 1,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Modify",
                    ClassAccess = "Create",
                    MaterialAccess = "View",
                    UserAccess = "Modify"
                },
                new PermissionModel
                {
                    PermissionId = 2,
                    RoleID = 2,
                    SyllabusAccess = "View",
                    ProgramAccess = "View",
                    MaterialAccess = "View",
                    UserAccess = "Modify",
                    ClassAccess = "View",
                }
            };

            var permissions = new List<Permission>
            {
                new Permission
                {
                    PermissionId = 1,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Full access",
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    PermissionId = 2,
                    RoleID = 2,
                    SyllabusAccess = "Modify",
                    ProgramAccess = "Modify",
                    ClassAccess = "Modify",
                    MaterialAccess = "Modify",
                    UserAccess = "Modify"
                }
            }.AsQueryable();

            _permissionRepository.FindByCondition(Arg.Any<Expression<Func<Permission, bool>>>())
                .Returns(permissions.AsQueryable());
            _permissionRepository.GetAll().Returns(permissions);
            _permissionRepository.GetByIdAsync(Arg.Any<int>())
                .Returns(callInfo =>
                {
                    // Lấy tham số truyền vào hàm GetByIdAsync (ID của Permission)
                    var permissionId = callInfo.Arg<int>();

                    // Tìm kiếm Permission tương ứng với ID trong danh sách dữ liệu giả
                    var permission = permissions.FirstOrDefault(p => p.PermissionId == permissionId);

                    // Trả về Permission hoặc null nếu không tìm thấy
                    return Task.FromResult(permission);
                });
            _permissionRepository.Update(Arg.Any<Permission>())
                .Returns(callInfo =>
                {
                    // Lấy tham số truyền vào hàm Update (Permission cần cập nhật)
                    var permissionToUpdate = callInfo.Arg<Permission>();

                    // Cập nhật dữ liệu trong danh sách giả với Permission mới
                    var existingPermission = permissions.FirstOrDefault(p => p.PermissionId == permissionToUpdate.PermissionId);
                    if (existingPermission != null)
                    {
                        existingPermission.SyllabusAccess = permissionToUpdate.SyllabusAccess;
                        existingPermission.ProgramAccess = permissionToUpdate.ProgramAccess;
                        existingPermission.ClassAccess = permissionToUpdate.ClassAccess;
                        existingPermission.MaterialAccess = permissionToUpdate.MaterialAccess;
                        existingPermission.UserAccess = permissionToUpdate.UserAccess;
                    }

                    return permissionToUpdate; // Trả về Permission đã được cập nhật
                });

            // Giả lập hàm Commit
            _permissionRepository.Commit()
                .Returns(callInfo =>
                {
                    // Trả về số lượng bản ghi đã được commit (đã cập nhật)
                    return Task.FromResult(permissions.Count());
                });
            // Act
            var updatedPermissions = await _permissionService.UpdatePermissions(permissionModels);

            // Assert
            updatedPermissions.Should().NotBeNull();
            updatedPermissions.Should().HaveCount(permissionModels.Count);
            // Add more assertions as needed
        }
        [Fact]
        public async Task UpdatePermissions_WithDuplicatePermissionId_ThrowsBadRequestException()
        {
            // Arrange
            var updatedPermissions = new List<PermissionModel>
            {
                new PermissionModel
                {
                    PermissionId = 1,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Modify",
                    ClassAccess = "Create",
                    MaterialAccess = "View",
                    UserAccess = "Modify"
                },
                new PermissionModel
                {
                    PermissionId = 1,
                    RoleID = 2,
                    SyllabusAccess = "View",
                    ProgramAccess = "View",
                    MaterialAccess = "View",
                    UserAccess = "Modify",
                    ClassAccess = "View",
                }
            };//Duplicate PermissionId

            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _permissionService.UpdatePermissions(updatedPermissions));
        }
        [Fact]
        public async Task UpdatePermissions_WithDuplicateRoleId_ThrowsBadRequestException()
        {
            // Arrange
            var updatedPermissions = new List<PermissionModel>
            {
                new PermissionModel { PermissionId = 1, RoleID = 1 },
                new PermissionModel { PermissionId = 2, RoleID = 1 } // Duplicate RoleId
            };

            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _permissionService.UpdatePermissions(updatedPermissions));
        }

        [Fact]
        public async Task UpdatePermissions_WithNonexistentPermission_ThrowsBadRequestException()
        {
            // Arrange
            var updatedPermissions = new List<PermissionModel>
            {
                new PermissionModel
                {
                    PermissionId = 100,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Full access",
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    UserAccess = "Full access"
                } // Nonexistent PermissionId
            };
            var permissions = new List<Permission>
            {
                new Permission
                {
                    PermissionId = 1,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Full access",
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    PermissionId = 2,
                    RoleID = 2,
                    SyllabusAccess = "Modify",
                    ProgramAccess = "Modify",
                    ClassAccess = "Modify",
                    MaterialAccess = "Modify",
                    UserAccess = "Modify"
                }
            }.AsQueryable();

            _permissionRepository.GetByIdAsync(Arg.Any<int>())
                .Returns(callInfo =>
                {
                    // Lấy tham số truyền vào hàm GetByIdAsync (ID của Permission)
                    var permissionId = callInfo.Arg<int>();

                    // Tìm kiếm Permission tương ứng với ID trong danh sách dữ liệu giả
                    var permission = permissions.FirstOrDefault(p => p.PermissionId == permissionId);

                    // Trả về Permission hoặc null nếu không tìm thấy
                    return Task.FromResult(permission);
                });


            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _permissionService.UpdatePermissions(updatedPermissions));
        }

        [Fact]
        public async Task UpdatePermissions_WithMismatchedRoleId_ThrowsBadRequestException()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission
                {
                    PermissionId = 1,
                    RoleID = 1,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Full access",
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    PermissionId = 2,
                    RoleID = 2,
                    SyllabusAccess = "Modify",
                    ProgramAccess = "Modify",
                    ClassAccess = "Modify",
                    MaterialAccess = "Modify",
                    UserAccess = "Modify"
                }
            }.AsQueryable();
            var updatedPermissions = new List<PermissionModel>
            {
                new PermissionModel
                {
                    PermissionId = 1,
                    RoleID = 2,
                    SyllabusAccess = "Full access",
                    ProgramAccess = "Modify",
                    ClassAccess = "Full access",
                    MaterialAccess = "Modify",
                    UserAccess = "Modify"
                } // Mismatched RoleId
            };

            _permissionRepository.GetByIdAsync(Arg.Any<int>())
                .Returns(callInfo =>
                {
                    // Lấy tham số truyền vào hàm GetByIdAsync (ID của Permission)
                    var permissionId = callInfo.Arg<int>();

                    // Tìm kiếm Permission tương ứng với ID trong danh sách dữ liệu giả
                    var permission = permissions.FirstOrDefault(p => p.PermissionId == permissionId);

                    // Trả về Permission hoặc null nếu không tìm thấy
                    return Task.FromResult(permission);
                });

            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _permissionService.UpdatePermissions(updatedPermissions));
        }
        [Fact]
        public async Task GetAllPermissions_ReturnsMappedPermissionModels()
        {
            // Arrange
            var permissions = new List<Permission>
    {
        new Permission
        {
            PermissionId = 1,
            RoleID = 1,
            SyllabusAccess = "Full access",
            ProgramAccess = "Modify",
            ClassAccess = "Create",
            MaterialAccess = "View",
            UserAccess = "Modify"
        },
        new Permission
        {
            PermissionId = 2,
            RoleID = 2,
            SyllabusAccess = "View",
            ProgramAccess = "View",
            ClassAccess = "View",
            MaterialAccess = "View",
            UserAccess = "Modify"
        }
    }.AsQueryable();

            // Setup mocked IRepository to return permissions
            _permissionRepository.GetAll().Returns(permissions.AsQueryable());

            // Act
            var result = await _permissionService.GetAllPermissions();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(permissions.Count());
            result.Should().OnlyContain(p => p != null);

        }
    }
}

