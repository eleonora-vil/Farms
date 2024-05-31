//using System.Linq.Expressions;
//using FluentAssertions;
//using Microsoft.Extensions.Options;
//using Mock_Project_Net03.Common.Payloads.Requests;
//using Mock_Project_Net03.Entities;
//using Mock_Project_Net03.Exceptions;
//using Mock_Project_Net03.Helpers;
//using Mock_Project_Net03.Repositories;
//using Mock_Project_Net03.Services;
//using Mock_Project_Net03.Settings;
//using NSubstitute;

//namespace Mock_Project_Net03.Tests.Unit.Services;

//public class IdentityServiceTests
//{
//    private readonly IOptions<JwtSettings> _jwtSettings = Substitute.For<IOptions<JwtSettings>>();
//    private readonly IRepository<User, int> _userRepository = Substitute.For<IRepository<User, int>>();
//    private readonly IRepository<UserRole, int> _userRoleRepository = Substitute.For<IRepository<UserRole, int>>();
//    private readonly IdentityService _sut;

//    public IdentityServiceTests()
//    {
//        _jwtSettings.Value.Returns(new JwtSettings()
//        {
//            Key = "??/fasdjfsdakklsfaddsafjhkdgsajkhasdhgkhjasdghasdghkasjdkg",
//        });
//        _sut = new IdentityService(_jwtSettings, _userRepository, _userRoleRepository);
//    }

//    [Fact]
//    public async Task IdentityService_Signup_Success()
//    {
//        // Arrange
//        var req = new SignupRequest
//        {
//            Email = "test@gmail.com",
//            Password = "test",
//        };
//        _userRepository.FindByCondition(Arg.Any<Expression<Func<User, bool>>>()).Returns(new List<User>().AsQueryable());
//        _userRepository.AddAsync(Arg.Any<User>()).Returns(new User());
//        _userRepository.Commit().Returns(1);

//        // Act
//        var result = await _sut.Signup(req);

//        // Assert
//        result.Should().Be(true);
//    }
    
//    [Fact]
//    public async Task IdentityService_Signup_Fail()
//    {
//        // Arrange
//        var req = new SignupRequest
//        {
//            Email = "test@gmail.com",
//            Password = "test",
//        };
//        _userRepository.FindByCondition(Arg.Any<Expression<Func<User, bool>>>()).Returns(new List<User>()
//        {
//            new ()
//        }.AsQueryable());

//        // Act
//        var action = async () => await _sut.Signup(req);

//        // Assert
//        await action.Should().ThrowAsync<BadRequestException>().WithMessage("username or email already exists");
//    }

//    [Fact]
//    public void IdentityService_Login_Success()
//    {
//        // Arrange
//        var email = "test@gmail.com";
//        var password = "test";
//        var hashedPassword = SecurityUtil.Hash(password);
//        _userRepository.FindByCondition(Arg.Any<Expression<Func<User, bool>>>()).Returns(new List<User>()
//        {
//            new ()
//            {
//                UserId = 1,
//                Password = hashedPassword,
//                Email = email,
//                UserName = "cac",
//            }
//        }.AsQueryable());
//        _userRoleRepository.FindByCondition(Arg.Any<Expression<Func<UserRole, bool>>>()).Returns(new List<UserRole>()
//        {
//            new ()
//            {
//                RoleName = "???/"
//            }
//        }.AsQueryable());
        
//        // Act
//        var result =  _sut.Login(email, password);
        
//        // Assert
//        result.Authenticated.Should().BeTrue();
//    }

//    [Fact]
//    public void IdentityService_Login_UserDoesNotExist()
//    {
//        // Arrange
//        var email = "test@gmail.com";
//        var password = "test";
//        _userRepository.FindByCondition(Arg.Any<Expression<Func<User, bool>>>()).Returns(new List<User>().AsQueryable());

//        // Act
//        var result = _sut.Login(email, password);

//        // Assert
//        result.Authenticated.Should().BeFalse();
//        result.Token.Should().BeNull();
//    }
    
//    [Fact]
//    public void IdentityService_Login_WrongPassword()
//    {
//        // Arrange
//        var email = "test@gmail.com";
//        var password = "test";
//        var hashedPassword = SecurityUtil.Hash(password);
//        var wrongPassword = "wrong";
//        _userRepository.FindByCondition(Arg.Any<Expression<Func<User, bool>>>()).Returns(new List<User>()
//        {
//            new ()
//            {
//                Password = hashedPassword,
//            }
//        }.AsQueryable());
//        _userRoleRepository.FindByCondition(Arg.Any<Expression<Func<UserRole, bool>>>()).Returns(new List<UserRole>()
//        {
//            new ()
//            {
//                RoleName = "???/"
//            }
//        }.AsQueryable());
        
//        // Act
//        var result =  _sut.Login(email, wrongPassword);
        
//        // Assert
//        result.Authenticated.Should().BeFalse();
//        result.Token.Should().BeNull();
//    }
//}