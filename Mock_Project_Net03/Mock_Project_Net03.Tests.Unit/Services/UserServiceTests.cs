using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Repositories;
using Mock_Project_Net03.Services;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mock_Project_Net03.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly CloudService _cloud;
        private readonly IRepository<User, int> _userRepo = Substitute.For<IRepository<User, int>>();
        private readonly IRepository<UserRole, int> _userRoleRepo = Substitute.For<IRepository<UserRole, int>>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();

        public UserServiceTests()
        {
            _sut = new UserService(_userRepo, _mapper, _userRoleRepo, _cloud);
        }

        [Fact]
        public async void UserService_CreateUser_ReturnsUserModel()
        {
            var testUser = new UserModel()
            {
                RoleID = 1,
                UserName = "A",
                FullName = "abc",
                Email = "test9@gmail.com",
                Gender = "Female",
                Level = "Student",
                Address = "HCM",
                BirthDate = DateTime.Now,
                PhoneNumber = "1234567890",
                Status = "true"
            };
            var role = new UserRole()
            {
                RoleId = 1,
                RoleName = "A",
            };
            var id = new Random().Next();
            var userEntity = new User()
            {
            };
            var addedUser = new User()
            {
                UserId = id,
            };
            _mapper.Map<User>(testUser).Returns(userEntity);
            _userRepo.FindByCondition(x => x.Email == testUser.Email).ReturnsNull();
            _userRoleRepo.FindByCondition(ur => ur.RoleId == testUser.RoleID).Returns(new List<UserRole>() { role }.AsQueryable());

            _userRepo.AddAsync(userEntity).Returns(addedUser);
            _userRepo.Commit().Returns(1);

            var result = await _sut.CreateNewUser(testUser);

            var expected = testUser;
            expected.UserId = id;
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async void UserService_CreateUser_WithDuplicatedEmail()
        {
            // Arrange
            var testUser = new UserModel()
            {
                UserName = "A",
                FullName = "abc",
                Email = "test@gmail.com",
                Gender = "Female",
                Level = "Student",
                Address = "HCM",
                BirthDate = DateTime.Now,
                PhoneNumber = "0123456789",
                Status = "true"
            };

            var duplicatedUserEntity = new User()
            {
                Email = testUser.Email
            };

            // Mock setup 
            _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
                     .Returns(new List<User> { duplicatedUserEntity }.AsQueryable());

            // Act
            Func<Task> action = async () => await _sut.CreateNewUser(testUser);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>().WithMessage("email already exist");
        }

        [Fact]
        public async void UserService_ChangeStatus_ReturnTrue()
        {
            int id = 1;
            var testUser = new User()
            {
                UserId = 1,
            };
            var changeStatus = testUser;
            changeStatus.Status = "InActive";
            _userRepo.GetByIdAsync(id).Returns(testUser);
            testUser.Status = "InActive";
            _userRepo.Update(testUser).Returns(changeStatus);
            _userRepo.Commit().Returns(1);
            var result = await _sut.ChangeStatus(id, changeStatus.Status);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenUserDeletedSuccessfully()
        {
            int id = 1;
            var testUser = new User()
            {
                UserId = 1,
            };
            var deletedUser = testUser;
            _userRepo.GetByIdAsync(id).Returns(testUser);
            _userRepo.Remove(testUser.UserId).Returns(deletedUser);
            _userRepo.Commit().Returns(1);
            var result = await _sut.DeleteUser(id);
            result.Should().BeTrue();
        }



        [Fact]
        public async void UserService_DeleteUser_ThrowsBadRequestException()
        {
            // Arrange
            int nonExistentUserId = 100;
            var user = _userRepo.GetByIdAsync(nonExistentUserId).Returns(Task.FromResult<User>(null));

            Func<Task> result = async () => await _sut.DeleteUser(nonExistentUserId);
            await result.Should().ThrowAsync<BadRequestException>().WithMessage("Can Not Find The User To Delete");

        }

        //UpdateUser
        [Fact]
        public async Task UpdateUser_WhenUserExists_ReturnsUpdatedUserModel()
        {
            // Arrange
            var updateUserModel = new UserModel
            {
                Email = "test@gmail.com"
            };

            var updateUserRequest = new UpdateUserRequest
            {
                PhoneNumber = "0123456789"
            };

            User existingUserEntity = new User
            {
                Email = "test@gmail.com"
            };

            var updatedUserEntity = new User
            {
                UserId = 1,
                PhoneNumber = "0123456789"
            };

            _mapper.Map<User>(updateUserModel).Returns(existingUserEntity);

            _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
                           .Returns(new List<User>() { existingUserEntity }.AsQueryable());

            //_mapper.Map(existingUserEntity, updateUserModel ).Returns(updatedUserEntity);
            _userRepo.Update(Arg.Any<User>()).Returns(updatedUserEntity);
            _userRepo.Commit().Returns(1);
            updateUserModel.PhoneNumber = updatedUserEntity.PhoneNumber;
            var expect = updateUserModel;
            _mapper.Map<UserModel>(updatedUserEntity).Returns(expect);

            // Act
            var result = await _sut.UpdateUser(updateUserModel, updateUserRequest);

            result.Should().BeEquivalentTo(expect);
        }

        [Fact]
        public async Task UpdateUser_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var updateUserModel = new UserModel
            {
                Email = "nonexistent@gmail.com"
            };

            var updateUserRequest = new UpdateUserRequest
            {
                PhoneNumber = "0123456789"
            };

            _mapper.Map<User>(updateUserModel).Returns(new User());
            _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
                .Returns(new List<User>().AsQueryable());

            // Act
            var result = await _sut.UpdateUser(updateUserModel, updateUserRequest);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public async Task ChangeUserRole_WithValidData_ShouldReturnUpdatedUserModel()
        {
            var roleIdFact3 = 2;
            var userUpdate = new UserModel
            {
                UserId = 1,
                Status = "Active",
                RoleID = 2,
                RoleName = "Admin",
            };
            var existingUserEntity = new User
            {
                UserId = 1,
                Status = "Active",
                RoleID = 3,
                UserRole = new UserRole
                {
                    RoleId = 3,
                    RoleName = "Instructor"
                }
            };
            var userRole = new UserRole
            {
                RoleId = roleIdFact3,
                RoleName = "Admin"
            };
            var updatedUserEntity = new User
            {
                UserId = 1,
                Status = "Active",
                RoleID = roleIdFact3,
                
                UserRole = new UserRole
                {
                    RoleId = roleIdFact3,
                    RoleName = "Admin"
                }
            };

            _mapper.Map<User>(userUpdate).Returns(existingUserEntity);
            _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
                               .Returns(new List<User> { existingUserEntity }.AsQueryable());
            _userRoleRepo.FindByCondition(Arg.Any<Expression<Func<UserRole, bool>>>())
                                    .Returns(new List<UserRole> { userRole }.AsQueryable());
            _userRepo.Update(Arg.Any<User>()).Returns(updatedUserEntity);
            _userRepo.Commit().Returns(1);
            var expect = updatedUserEntity;

            var resultFact3 = await _sut.ChangeUserRole(userUpdate, userRole.RoleId);

            resultFact3.Should().BeEquivalentTo(expect, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task ChangeUserRole_WithInvalidRoleId_ReturnsBadRequestException()
        {
            var roleId = 0;
            var updateUserRoleModel = new UserModel
            {
                UserId = 1,
                Status = "Active"
            };
            var userRole = new UserRole
            {
                RoleId = roleId,
                RoleName = "RoleName"
            };
            var existingUserEntity = new User
            {
                UserId = 1,
                RoleID = 3,
                Status = "Active"
            };

            _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
                                   .Returns(new List<User> { existingUserEntity }.AsQueryable());
            _userRoleRepo.FindByCondition(Arg.Any<Expression<Func<UserRole, bool>>>())
                                   .Returns(new List<UserRole> { null }.AsQueryable());

            Func<Task> result = async () => await _sut.ChangeUserRole(updateUserRoleModel, userRole.RoleId);
            await result.Should().ThrowAsync<BadRequestException>();
        }

        //[Fact]
        //public async Task ChangeUserRole_WithNonExistentUser_ReturnsNotFoundException()
        //{
        //    var roleIdFact2 = 2;
        //    var updateUserRoleModel = new UserModel
        //    {
        //        UserId = 2,
        //        Status = "Active"
        //    };
        //    var userRole = new UserRole
        //    {
        //        RoleId = roleIdFact2,
        //        RoleName = "RoleName"
        //    };
        //    var existingUserEntity = new User
        //    {
        //        UserId = 1,
        //        RoleID = 3,
        //        Status = "Active"
        //    };

        //    _userRepo.FindByCondition(Arg.Any<Expression<Func<User, bool>>>())
        //                                 .Returns(new List<User>() { null }.AsQueryable());
        //    _userRoleRepo.FindByCondition(Arg.Any<Expression<Func<UserRole, bool>>>())
        //                                 .Returns(new List<UserRole> { userRole }.AsQueryable());

        //    Func<Task> action = async () => await _sut.ChangeUserRole(updateUserRoleModel, userRole.RoleId);
        //    await action.Should().ThrowAsync<NotFoundException>();
        //}

    }
}

