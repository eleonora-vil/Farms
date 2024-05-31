using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Helpers;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private readonly UserRoleService _userRoleService;
        private readonly PermissionService _permissionService;
        private readonly IdentityService _identityService;
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(UserService userService, UserRoleService userRoleService
            , PermissionService permissionService, IdentityService identityService, EmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _permissionService = permissionService;
            _identityService = identityService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1)
        {

            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("View") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new BadRequestException("Authorization header is missing or invalid.");
            }
            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;


            var results = await _userService.GetAllUsers(page, emailClaim, roleClaim);

            return Ok(ApiResult<GetUsersResponse>.Succeed(new GetUsersResponse
            {
                Users = results,
            }));
        }




        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateNewUserRequest req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("Create") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }
            var user = req.ToUserModel();
            UserValidator validations = new UserValidator();
            var valid = await validations.ValidateAsync(user);
            if (!valid.IsValid)
            {
                throw new RequestValidationException(valid.ToProblemDetails());
            }
            var userRole = await _userRoleService.GetByName(req.RoleName);

            user.RoleID = userRole.RoleId;
            var result = await _userService.CreateNewUser(user);
            /*            result.UserRole = userRole;*/
            if (result is not null)
            {
                return Ok(ApiResult<CreateUsersRespone>.Succeed(new CreateUsersRespone
                {
                    User = user
                }));
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("Update/{userId}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest req)
        {

            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("Modify") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }
            if (userId <= 0)
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid User Id"
                }));
            }

            var existingUser = await _userService.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "User not found"
                }));
            }


            //if (!valid.IsValid)
            //{
            //    throw new RequestValidationException(valid.ToProblemDetails());
            //}

            var updatedUser = await _userService.UpdateUser(existingUser, req);

            if (updatedUser != null)
            {
                return Ok(ApiResult<UpdateUserRespone>.Succeed(new UpdateUserRespone
                {
                    User = updatedUser
                }));
            }
            else
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid data provided"
                }));
            }
        }



        [HttpPut("ChangeRole/{roleId}/{userId}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> ChangeUserRole(int userId, int roleId)
        {

            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("Modify") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }
            if (userId <= 0)
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid User Id"
                }));
            }

            if (roleId <= 0 || roleId > 4)
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid User Id"
                }));
            }

            var existingUser = await _userService.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "User Not Found"
                }));
            }


            var updatedUser = await _userService.ChangeUserRole(existingUser, roleId);

            if (updatedUser != null)
            {
                return Ok(ApiResult<UpdateUserRespone>.Succeed(new UpdateUserRespone
                {
                    User = updatedUser
                }));
            }
            else
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Fail to change user role"
                }));
            }
        }

        [HttpPut("ChangeStatus/{userId}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> ChangeStatus(int userId, [FromBody] ChangeStatusResquest req)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("Modify") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }
            if (userId <= 0)
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid User Id"
                }));
            }

            var result = await _userService.ChangeStatus(userId, req.status);

            if (result)
            {
                return Ok(ApiResult<DeleteUserResponse>.Succeed(new DeleteUserResponse
                {
                    Message = "Change status user success"
                }));
            }
            else
            {
                return BadRequest(ApiResult<DeleteUserResponse>.Error(new DeleteUserResponse
                {
                    Message = "Change status user fail"
                }));
            }
        }


        [HttpGet("GetBy/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split()[1];
            var currentUser = await _userService.GetUserInToken(token);
            var permission = await _permissionService.GetPermissionByRoleID(currentUser.RoleID);
            if (!permission.UserAccess.Equals("View") && !permission.UserAccess.Equals("Full access"))
            {
                throw new BadRequestException("This account do not have permission");
            }
            if (userId <= 0)
            {
                return BadRequest(ApiResult<GetUserByIdRespone>.Error(new GetUserByIdRespone
                {
                    message = "Invalid UserId"
                }));
            }

            var result = await _userService.GetUserAndRole(userId);
            return Ok(ApiResult<GetUserByIdRespone>.Succeed(new GetUserByIdRespone
            {
                User = result,
            }));

        }


        [HttpPost("FirstStep")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> FirstStepResgisterInfo(FirstStepResquest req)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(req.Email))
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Invalid email format: " + req.Email
                }));
            }
            var otp = 0;
            var Password = SecurityUtil.GenerateRandomPassword(); ;

            var email = req.Email;
            var link = req.Link;
            var user = await _userService.GetUserByEmail(email);
            if (user != null && user.OTPCode == "0")
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Account Already Exists"
                }));
            }

            if (user != null && user.CreateDate > DateTime.Now && user.OTPCode != "0")
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "OTP Code is not expired"
                }));
            }

            if (user == null)
            {
                otp = new Random().Next(100000, 999999);
                var href = link + req.Email;
                var mailData = new MailData()
                {
                    EmailToId = email,
                    EmailToName = "KayC",
                    EmailBody = $@"
<div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
    <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
    <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
    <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
    <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{otp}</span></div>
    <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your Password is: <span style=""font-weight: bold; color: #e74c3c;"">{Password}</span></div>
    <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
</div>",
                    EmailSubject = "OTP Verification"
                };


                var result = await _emailService.SendEmailAsync(mailData);
                if (!result)
                {
                    throw new BadRequestException("Send Email Fail");
                }
            }
            var createUserModel = new CreateUserModel
            {
                Email = req.Email,
                OTPCode = otp.ToString(),
                Password = Password
            };
            var userModel = await _userService.FirstStep(createUserModel);

            if (userModel.OTPCode != otp.ToString())
            {
                var href = link + req.Email;
                var mailUpdateData = new MailData()
                {
                    EmailToId = email,
                    EmailToName = "KayC",
                    EmailBody = $@"
<div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
    <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
    <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
    <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
    <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{userModel.OTPCode}</span></div>
    <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your Password is: <span style=""font-weight: bold; color: #e74c3c;"">{Password}</span></div>
    <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
</div>",
                    EmailSubject = "OTP Verification"
                };
                var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                if (!rsUpdate)
                {
                    throw new BadRequestException("Send Email Fail");
                }
            }

            return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
            {
                message = "Check Email and Verify OTP",
            }));
        }

        [HttpPost("SubmitOTP")]
        public async Task<IActionResult> SubmitOTP(SubmitOTPResquest req)
        {
            var result = await _userService.SubmitOTP(req);
            if (!result)
            {
                throw new BadRequestException("OTP Code is not Correct");
            }
            return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
            {
                message = $"Create new Account Success for email: {req.Email}"
            }));
        }




        [HttpPut("UpdateInfo")]
        public async Task<IActionResult> UpdateInfo(string email, [FromForm] UpdateInfoRequest req)
        {

            /*            Request.Headers.TryGetValue("Authorization", out var token);
                        token = token.ToString().Split()[1];
                        var currentUser = await _userService.GetUserInToken(token);*/

            var currentUser = await _userService.GetUserByEmail(email);

            if (currentUser == null)
            {
                return NotFound(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "User not found"
                }));
            }


            var updatedUser = await _userService.UpdateInfoUser(currentUser, req);

            if (updatedUser != null)
            {
                return Ok(ApiResult<UpdateUserRespone>.Succeed(new UpdateUserRespone
                {
                    User = updatedUser,
                    message = "Update Info Success"
                }));
            }
            else
            {
                return BadRequest(ApiResult<UpdateUserRespone>.Error(new UpdateUserRespone
                {
                    message = "Invalid data provided"
                }));
            }
        }

        [HttpGet("GetTimeOTP")]
        public async Task<IActionResult> GetTimeOTP(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(ApiResult<GetTimeOTP>.Error(new GetTimeOTP
                {
                    Message = "User Not Found"
                }));
            }

            else
            {
                DateTimeOffset utcTime = DateTimeOffset.Parse(user.CreateDate.ToString());
                return Ok(ApiResult<GetTimeOTP>.Succeed(new GetTimeOTP
                {
                    Message = "Success",
                    EndTime = utcTime
                }));

            }
        }

        [HttpPost("CreateListAccount")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> CreateListAccount([FromBody] CreateListAccountResquest req)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            foreach (var email in req.emails)
            {
                if (!regex.IsMatch(email))
                {
                    return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                    {
                        message = "Invalid email format: " + email
                    }));
                }
                var otp = 0;
                var Password = SecurityUtil.GenerateRandomPassword();

                var user = await _userService.GetUserByEmail(email);
                if (user != null && user.OTPCode == "0")
                {
                    return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                    {
                        message = "Account Already Exists"
                    }));
                }

                if (user != null && DateTimeOffset.Now > user.CreateDate.Value.AddMinutes(-2) && DateTimeOffset.Now < user.CreateDate && user.OTPCode != "0")
                {
                    return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                    {
                        message = "OTP Code is not expired"
                    }));
                }

                if (user == null)
                {
                    var href = req.Link + email;
                    otp = new Random().Next(100000, 999999);
                    var mailData = new MailData()
                    {
                        EmailToId = email,
                        EmailToName = "KayC",
                        EmailBody = $@"
                    <div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
                        <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
                        <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
                        <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{otp}</span></div>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your Password is: <span style=""font-weight: bold; color: #e74c3c;"">{Password}</span></div>
                        <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
                    </div>",
                        EmailSubject = "OTP Verification"
                    };

                    var result = await _emailService.SendEmailAsync(mailData);
                    if (!result)
                    {
                        throw new BadRequestException("Send Email Fail");
                    }
                }

                var createUserModel = new CreateUserModel
                {
                    Email = email,
                    OTPCode = otp.ToString(),
                    Password = Password
                };
                var userModel = await _userService.FirstStep(createUserModel);

                if (userModel.OTPCode != otp.ToString())
                {
                    var href = req.Link + email;
                    var mailUpdateData = new MailData()
                    {
                        EmailToId = email,
                        EmailToName = "KayC",
                        EmailBody = $@"
                    <div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
                        <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
                        <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
                        <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{userModel.OTPCode}</span></div>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your Password is: <span style=""font-weight: bold; color: #e74c3c;"">{Password}</span></div>
                        <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
                    </div>",
                        EmailSubject = "OTP Verification"
                    };
                    var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                    if (!rsUpdate)
                    {
                        throw new BadRequestException("Send Email Fail");
                    }
                }
            }

            return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
            {
                message = "Check Email and Verify OTP",
            }));
        }

        [HttpPost("ReSendOTP")]
        public async Task<IActionResult> ReSendOTP([FromBody] FirstStepResquest req)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(req.Email))
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Invalid email format: " + req.Email
                }));
            }
            var user = await _userService.GetUserByEmail(req.Email);
            if (user == null)
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Account does not exist"
                }));
            }
            var twoMinuteAgo = user.CreateDate.Value.AddMinutes(-2);
            if (user != null && DateTimeOffset.Now > twoMinuteAgo && DateTimeOffset.Now < user.CreateDate && user.OTPCode != "0")
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "OTP Code is not expired"
                }));
            }
            var otp = new Random().Next(100000, 999999); ;
            var Password = SecurityUtil.GenerateRandomPassword();
            var createUserModel = new CreateUserModel
            {
                Email = req.Email,
                OTPCode = otp.ToString(),
                Password = Password
            };
            var userModel = await _userService.FirstStep(createUserModel);
            if (userModel != null)
            {
                var href = req.Link + req.Email;
                var mailData = new MailData()
                {
                    EmailToId = req.Email,
                    EmailToName = "KayC",
                    EmailBody = $@"
                    <div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
                        <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
                        <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
                        <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{userModel.OTPCode}</span></div>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your Password is: <span style=""font-weight: bold; color: #e74c3c;"">{Password}</span></div>
                        <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
                    </div>",
                    EmailSubject = "OTP Verification"
                };

                var result = await _emailService.SendEmailAsync(mailData);
                if (!result)
                {
                    throw new BadRequestException("Send Email Fail");
                }
                return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
                {
                    message = "Check Email and Verify OTP",
                }));
            }

            return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
            {
                message = "Resend OTP fail",
            }));

        }

        [HttpPost("ForgotPass")]
        public async Task<IActionResult> ForgotPass([FromBody] FirstStepResquest req)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(req.Email))
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Invalid email format: " + req.Email
                }));
            }
            var user = await _userService.GetUserByEmail(req.Email);
            if (user == null)
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Account does not exist"
                }));
            }

            if (user.Status == "Active")
            {
                var twoMinuteAgo = user.CreateDate.Value.AddMinutes(-2);
                if (user != null && DateTimeOffset.Now > twoMinuteAgo && DateTimeOffset.Now < user.CreateDate)
                {
                    return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                    {
                        message = "OTP Code is not expired"
                    }));
                }
                var otp = new Random().Next(100000, 999999); ;
                var Password = SecurityUtil.GenerateRandomPassword();
                var createUserModel = new CreateUserModel
                {
                    Email = req.Email,
                    OTPCode = otp.ToString(),
                    Password = Password
                };
                var userModel = await _userService.ForgotPass(createUserModel);
                if (userModel != null)
                {
                    var href = req.Link + req.Email;
                    var mailData = new MailData()
                    {
                        EmailToId = req.Email,
                        EmailToName = "KayC",
                        EmailBody = $@"
                    <div style=""max-width: 400px; margin: 50px auto; padding: 30px; text-align: center; font-size: 120%; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 20px rgba(0, 0, 0, 0.1); position: relative;"">
                        <img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTRDn7YDq7gsgIdHOEP2_Mng6Ym3OzmvfUQvQ&usqp=CAU"" alt=""Noto Image"" style=""max-width: 100px; height: auto; display: block; margin: 0 auto; border-radius: 50%;"">
                        <h2 style=""text-transform: uppercase; color: #3498db; margin-top: 20px; font-size: 28px; font-weight: bold;"">Welcome to Team 3</h2>
                        <a href=""{href}"" style=""display: inline-block; background-color: #3498db; color: #fff; text-decoration: none; padding: 10px 20px; border-radius: 5px; margin-bottom: 20px;"">Click here to verify</a>
                        <div style=""font-size: 18px; color: #555; margin-bottom: 30px;"">Your OTP Code is: <span style=""font-weight: bold; color: #e74c3c;"">{userModel.OTPCode}</span></div>
                        <p style=""color: #888; font-size: 14px;"">Powered by Team 3</p>
                    </div>",
                        EmailSubject = "OTP Verification"
                    };

                    var result = await _emailService.SendEmailAsync(mailData);
                    if (!result)
                    {
                        throw new BadRequestException("Send Email Fail");
                    }
                    return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
                    {
                        message = "Check Email and Verify OTP",
                    }));
                }
            }
            else
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Account has not been verified",
                }));
            }

            return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
            {
                message = "Resend OTP fail",
            }));

        }

        [HttpPatch("UpdatePass")]
        public async Task<IActionResult> UpdatePassword(UpdatePassResquest req)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(req.Email))
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Invalid email format: " + req.Email
                }));
            }
            var user = await _userService.GetUserByEmail(req.Email);
            if (user == null)
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Account does not exist"
                }));
            }

            var result = await _userService.UpdatePass(req);
            if (result == true)
            {
                return Ok(ApiResult<FirstStepResgisterInfoResponse>.Succeed(new FirstStepResgisterInfoResponse
                {
                    message = "Update password success",
                }));
            }
            else
            {
                return BadRequest(ApiResult<FirstStepResgisterInfoResponse>.Error(new FirstStepResgisterInfoResponse
                {
                    message = "Update password fail",
                }));
            }
            return null;
        }

        [HttpGet("UserInActive")]

        public async Task<IActionResult> GetUserInActive()
        {
            var listUser = await _userService.GetUserInActive();
            if (listUser == null)
            {
                return BadRequest(ApiResult<GetUserInActiveRespone>.Error(new GetUserInActiveRespone
                {
                    messages = "Not user InActive"
                }));
            }
            return Ok((ApiResult<GetUserInActiveRespone>.Error(new GetUserInActiveRespone
            {
                Users = listUser,
                messages = "List User is Active"
            })));
        }
    }

}
