using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class CreateNewUserRequest
    {

        public string CurrentUserName { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string Level { get; set; }

        public string Address { get; set; }

        public string BirthDate { get; set; }

        public string PhoneNumber { get; set; }
        public string Status { get; set; }

    }

    public static class UserRequestExtenstion
    {
        public static UserModel ToUserModel(this CreateNewUserRequest req) 
        {
            DateTime validDate;
            var UserModel = new UserModel()
            {
                UserName = req.UserName,
                // Password = SecurityUtil.Hash(req.Password),
                FullName = req.FullName,
                Gender = req.Gender,
                Level = req.Level,
                Address = req.Address,
                BirthDate = DateTime.TryParse(req.BirthDate, out validDate)?validDate:(DateTime?)null,
                CreateBy = req.CurrentUserName,
                CreateDate = DateTime.Now,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                Status = req.Status
            };    
            return UserModel;
        }
        public static UserRoleModel ToUserRoleModel(this CreateNewUserRequest req) 
        {
            var userRole = new UserRoleModel()
            {
                RoleName = req.RoleName,
            };
            return userRole;
        }

    }

}
