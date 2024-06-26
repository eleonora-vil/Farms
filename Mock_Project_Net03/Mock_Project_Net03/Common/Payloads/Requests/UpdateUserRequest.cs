﻿using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class 
        UpdateUserRequest
    {
        public string? CurrentUserName { get; set; }
        public string? UserName { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        [MaxLength(50)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Gender { get; set; }

        [MaxLength(100)]
        public string? Level { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
    }

    public static class UpdateUserRequestExtenstion
    {
        public static UserModel ToUserModel(this UpdateUserRequest req)
        {
            var userModel = new UserModel()
            {
                UserName = req.UserName,
                FullName = req.FullName,
                Gender = req.Gender,
                Level = req.Level,
                Address = req.Address,
                ModifyBy = req.CurrentUserName,
                ModifyDate = DateTime.Now,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
            };

            // if (!string.IsNullOrEmpty(req.Password))
            // {
            //     userModel.Password = SecurityUtil.Hash(req.Password);
            // }

            // Kiểm tra và thiết lập ngày sinh
            if (req.BirthDate.HasValue)
            {
                userModel.BirthDate = req.BirthDate.Value;
            }
            else
            {
                userModel.BirthDate = DateTime.Now;
            }

            return userModel;
        }
    }
}
