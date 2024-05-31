using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Helpers;
using Mock_Project_Net03.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace Mock_Project_Net03.Services
{
    public class UserService
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<UserRole, int> _userRoleRepository;
        private readonly CloudService _cloudService;
        public static int Page_Size { get; set; } = 10;

        public UserService(IRepository<User, int> userRepository, IMapper mapper, IRepository<UserRole, int> userRoleRepository, CloudService cloudService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _cloudService = cloudService;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers(int page, string email, string role)
        {
            IQueryable<User> listUser;

            if (role.Equals("Super Admin"))
            {
                listUser = _userRepository.GetAll().Where(x => x.Email != email && x.Status == "Active");
            }
            else if (role.Equals("Admin"))
            {
                listUser = _userRepository.GetAll().Where(x => x.Email != email && x.Status == "Active" && x.RoleID != 1);
            }
            else
            {
                return Enumerable.Empty<UserModel>();
            }

            listUser = listUser.Include(x => x.UserRole);

            var userModelList = await listUser.ToListAsync(); // ToListAsync() executes the query

            var userModelMappedList = _mapper.Map<List<UserModel>>(userModelList);

            foreach (var userModel in userModelMappedList)
            {
                var userEntity = userModelList.FirstOrDefault(x => x.UserId == userModel.UserId);
                if (userEntity != null)
                {
                    userModel.RoleName = userEntity.UserRole.RoleName;
                }
            }

            return userModelMappedList;
        }


        public async Task<UserModel> CreateNewUser(UserModel newUser)
        {
            var userEntity = _mapper.Map<User>(newUser);
            var existedUser = _userRepository.FindByCondition(x => x.Email == newUser.Email).FirstOrDefault();
            if (existedUser is not null)
            {
                throw new BadRequestException("email already exist");
            }
            var userRoleEntity = _userRoleRepository.FindByCondition(ur => ur.RoleId == newUser.RoleID).FirstOrDefault();

            userEntity.UserRole = userRoleEntity!;
            userEntity.Status = "Active";

            userEntity = await _userRepository.AddAsync(userEntity);
            int result = await _userRepository.Commit();
            if (result > 0)
            {
                // get latest userID
                //newUser.UserId = _userRepository.GetAll().OrderByDescending(x => x.);
                newUser.UserId = userEntity.UserId;
                return newUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userEntity = _mapper.Map<UserModel>(user);
            if (user is not null)
            {
                return userEntity;
            }
            return null;
        }

        public async Task<UserModel> UpdateUser(UserModel userUpdate, UpdateUserRequest req)
        {
            try
            {
                var userEntity = _mapper.Map<User>(userUpdate);

                var existedUser = _userRepository.FindByCondition(x => x.Email == userEntity.Email).FirstOrDefault();

                if (existedUser != null)
                {
                    // Cập nhật thông tin người dùng từ req
                    if (!string.IsNullOrEmpty(req.UserName))
                    {
                        existedUser.UserName = req.UserName;
                    }
                    if (!string.IsNullOrEmpty(req.Password))
                    {
                        existedUser.Password = SecurityUtil.Hash(req.Password);
                    }
                    if (!string.IsNullOrEmpty(req.FullName))
                    {
                        if (!IsValidFullName(req.FullName))
                        {
                            return null;
                        }
                        existedUser.FullName = req.FullName;
                    }
                    if (!string.IsNullOrEmpty(req.Email))
                    {
                        if (!IsValidEmail(req.Email))
                        {
                            return null;
                        }
                        existedUser.Email = req.Email;
                    }
                    if (!string.IsNullOrEmpty(req.Gender))
                    {
                        existedUser.Gender = req.Gender;
                    }
                    if (!string.IsNullOrEmpty(req.Level))
                    {
                        existedUser.Level = req.Level;
                    }
                    if (!string.IsNullOrEmpty(req.Address))
                    {
                        existedUser.Address = req.Address;
                    }
                    if (req.BirthDate.HasValue)
                    {
                        existedUser.BirthDate = req.BirthDate;
                    }
                    if (!string.IsNullOrEmpty(req.PhoneNumber))
                    {
                        if (!IsValidPhoneNumber(req.PhoneNumber))
                        {
                            return null;
                        }
                        existedUser.PhoneNumber = req.PhoneNumber;
                    }

                    //_mapper.Map(userEntity, existedUser);

                    var user = _userRepository.Update(existedUser);

                    var result = await _userRepository.Commit();

                    if (result > 0)
                    {
                        return _mapper.Map<UserModel>(user);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return null;
            }
        }

        public async Task<UserModel> UpdateInfoUser(UserModel userUpdate, UpdateInfoRequest req)
        {
            try
            {
                var userEntity = _mapper.Map<User>(userUpdate);

                var existedUser = _userRepository.FindByCondition(x => x.Email == userEntity.Email).Include(x => x.UserRole).FirstOrDefault();

                if (existedUser != null)
                {
                    // Cập nhật thông tin người dùng từ req
                    if (!string.IsNullOrEmpty(req.UserName))
                    {
                        existedUser.UserName = req.UserName;
                        existedUser.ModifyBy = req.UserName;
                    }
                    if (!string.IsNullOrEmpty(req.Password))
                    {
                        existedUser.Password = SecurityUtil.Hash(req.Password);
                    }
                    if (!string.IsNullOrEmpty(req.FullName))
                    {
                        if (!IsValidFullName(req.FullName))
                        {
                            return null;
                        }
                        existedUser.FullName = req.FullName;
                    }
                    if (!string.IsNullOrEmpty(req.Gender))
                    {
                        existedUser.Gender = req.Gender;
                    }
                    if (!string.IsNullOrEmpty(req.Level))
                    {
                        existedUser.Level = req.Level;
                    }
                    if (!string.IsNullOrEmpty(req.Address))
                    {
                        existedUser.Address = req.Address;
                    }
                    if (req.BirthDate.HasValue)
                    {
                        existedUser.BirthDate = req.BirthDate;
                    }
                    if (!string.IsNullOrEmpty(req.PhoneNumber))
                    {
                        if (!IsValidPhoneNumber(req.PhoneNumber))
                        {
                            return null;
                        }
                        existedUser.PhoneNumber = req.PhoneNumber;
                    }
                    if (req.Avatar != null)
                    {
                        var uploadResult = await _cloudService.UploadImageAsync(req.Avatar);

                        if (uploadResult.Error == null)
                        {
                            existedUser.Avatar = uploadResult.SecureUrl.ToString();
                        }
                        else
                        {
                            Console.WriteLine("Failed to upload avatar image");
                        }
                    }

                    //_mapper.Map(userEntity, existedUser);
                    existedUser.ModifyDate = DateTime.Now;

                    var user = _userRepository.Update(existedUser);

                    var result = await _userRepository.Commit();

                    var UserModel = new UserModel()
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Gender = user.Gender,
                        Level = user.Level,
                        Address = user.Address,
                        BirthDate = user.BirthDate,
                        ModifyBy = user.ModifyBy,
                        ModifyDate = user.ModifyDate,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Status = user.Status,
                        Avatar = user.Avatar,
                        RoleID = user.RoleID,
                        RoleName = user.UserRole.RoleName
                    };

                    if (result > 0)
                    {
                        return UserModel;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return null;
            }
        }

        private bool IsValidFullName(string fullName)
        {
            return Regex.IsMatch(fullName, "^[a-zA-Z\\s]*$");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^0\\d{9,11}$");
        }

        public async Task<UserModel> ChangeUserRole(UserModel userUpdate, int roleId)
        {
            var userEntity = _mapper.Map<User>(userUpdate);

            var existedUser = _userRepository.FindByCondition(x => x.UserId == userEntity.UserId)
                                              .Include(x => x.UserRole)
                                              .FirstOrDefault();
            var checkInActive = existedUser.Status.ToLower();

            if (checkInActive == "inactive")
            {
                throw new NotFoundException("User is inactive");
            }

            if (existedUser is null)
            {
                throw new NotFoundException("User not found");
            }

            var userRole = _userRoleRepository.FindByCondition(x => x.RoleId == roleId)
                                               .FirstOrDefault();

            if (userRole is null)
            {
                throw new BadRequestException("Invalid role ID");
            }

            if (roleId > 0 && roleId <= 4 && existedUser.RoleID != roleId)
            {
                existedUser.RoleID = roleId;
                existedUser.UserRole = userRole;
            }

            var user = _userRepository.Update(existedUser);

            var userWithRole = new UserModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                FullName = user.FullName,
                Email = user.Email,
                Gender = user.Gender,
                Level = user.Level,
                Address = user.Address,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                FSU = user.FSU,
                CreateBy = user.CreateBy,
                CreateDate = user.CreateDate,
                ModifyBy = user.ModifyBy,
                ModifyDate = user.ModifyDate,
                Status = user.Status.ToString(),
                RoleID = user.UserRole.RoleId,
                RoleName = user.UserRole.RoleName
            };

            var result = await _userRepository.Commit();

            if (result > 0)
            {
                return userWithRole;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ChangeStatus(int userId, string status)
        {

            var userChange = await _userRepository.GetByIdAsync(userId);
            if (userChange != null)
            {
                if (status == "Active" || status == "InActive")
                {
                    userChange.Status = status;
                    _userRepository.Update(userChange);
                }
                else
                {
                    throw new BadRequestException("Invalid provided status");
                }
                var rs = await _userRepository.Commit();
                return rs > 0;
            }
            else
            {
                throw new BadRequestException("Can Not Find The User To Change");
            }

        }
        public async Task<bool> DeleteUser(int userId)
        {

            var userDelete = await _userRepository.GetByIdAsync(userId);
            if (userDelete != null)
            {
                var user = _userRepository.Remove(userId);
                var rs = await _userRepository.Commit();
                return rs > 0;
            }
            else
            {
                throw new BadRequestException("Can Not Find The User To Delete");
            }

        }

        public async Task<UserModel> GetUserAndRole(int userId)
        {
            try
            {
                var user = await _userRepository.FindByCondition(x => x.UserId == userId)
                                                .Include(x => x.UserRole)
                                                .FirstOrDefaultAsync();

                if (user != null)
                {
                    // Tạo một đối tượng mới để chứa thông tin user và roleName của userRole
                    var userWithRole = new UserModel
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Password = user.Password,
                        FullName = user.FullName,
                        Email = user.Email,
                        Gender = user.Gender,
                        Level = user.Level,
                        Address = user.Address,
                        BirthDate = user.BirthDate,
                        PhoneNumber = user.PhoneNumber,
                        FSU = user.FSU,
                        CreateBy = user.CreateBy,
                        CreateDate = user.CreateDate,
                        ModifyBy = user.ModifyBy,
                        ModifyDate = user.ModifyDate,
                        Status = user.Status.ToString(),
                        RoleID = user.UserRole.RoleId,
                        RoleName = user.UserRole.RoleName
                    };

                    return userWithRole;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            return null;
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return _mapper.Map<UserModel>(_userRepository.FindByCondition(x => x.Email == email).FirstOrDefault());
        }

        public async Task<UserModel> GetUserInToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new BadRequestException("Authorization header is missing or invalid.");
            }
            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Check if the token is expired
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                throw new BadRequestException("Token has expired.");
            }

            string email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var user = await _userRepository.FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new BadRequestException("Can not found User");
            }
            return _mapper.Map<UserModel>(user);
        }

        public async Task<CreateUserModel> FirstStep(CreateUserModel req)
        {
            var userEntity = _mapper.Map<User>(req);
            var user = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

            if (user != null && user.OTPCode != "0" && user.Status == "InActive")
            {
                user.RoleID = 4;
                user.Status = "InActive";
                user.CreateDate = DateTimeOffset.Now.AddMinutes(2);
                user.Password = SecurityUtil.Hash(req.Password);
                user.OTPCode = new Random().Next(100000, 999999).ToString();

                user = _userRepository.Update(user);
                int rs = await _userRepository.Commit();
                if (rs > 0)
                {
                    return _mapper.Map<CreateUserModel>(user);
                }
                else
                {
                    return null;
                }
            }
            userEntity.RoleID = 4;
            userEntity.Status = "InActive";
            userEntity.CreateDate = DateTimeOffset.Now.AddMinutes(2);
            userEntity.Password = SecurityUtil.Hash(req.Password);
            var existedUser = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();
            if (existedUser != null)
            {
                throw new BadRequestException("email already exist");
            }
            userEntity = await _userRepository.AddAsync(userEntity);
            int result = await _userRepository.Commit();
            if (result > 0)
            {
                // get latest userID
                //newUser.UserId = _userRepository.GetAll().OrderByDescending(x => x.);
                req.UserId = userEntity.UserId;
                return _mapper.Map<CreateUserModel>(userEntity);
            }
            else
            {
                return null;
            }
        }

        public async Task<CreateUserModel> ForgotPass(CreateUserModel req)
        {
            var userEntity = _mapper.Map<User>(req);
            var user = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

            if (user != null  && user.Status == "Active")
            {
                user.RoleID = 4;
                user.CreateDate = DateTimeOffset.Now.AddMinutes(2);
                user.OTPCode = new Random().Next(100000, 999999).ToString();

                user = _userRepository.Update(user);
                int rs = await _userRepository.Commit();
                if (rs > 0)
                {
                    return _mapper.Map<CreateUserModel>(user);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdatePass(UpdatePassResquest req)
        {
            var user = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

            if (user != null && user.Status == "Active")
            {
                user.RoleID = 4;
                user.ModifyDate = DateTimeOffset.Now.AddMinutes(2);
                user.Password = SecurityUtil.Hash(req.Password);

                user = _userRepository.Update(user);
                int rs = await _userRepository.Commit();
                if (rs > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        public async Task<bool> SubmitOTP(SubmitOTPResquest req)
        {
            var existedUser = _userRepository.FindByCondition(x => x.Email == req.Email).FirstOrDefault();

            if (existedUser is null)
            {
                throw new BadRequestException("Account does not exist");
            }

            if (req.OTP != existedUser.OTPCode)
            {
                throw new BadRequestException("OTP Code is not Correct");
            }

            var result = 0;

            var twoMinuteAgo = existedUser.CreateDate.Value.AddMinutes(-2);

            if (DateTimeOffset.Now > twoMinuteAgo && DateTimeOffset.Now < existedUser.CreateDate)
            {
                existedUser.Status = "Active";
                existedUser.OTPCode = "0";
                _userRepository.Update(existedUser);
                result = await _userRepository.Commit();
            }
            else
            {
                throw new BadRequestException("OTP code is expired");
            }

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<UserModel>> GetUserInActive()
        {
            var listUser = _userRepository.GetAll().Where(x => x.Status == "InActive");
            if (listUser.Count() > 0)
            {
                return _mapper.Map<List<UserModel>>(listUser);
            }
            return null;
        }

    }

}
