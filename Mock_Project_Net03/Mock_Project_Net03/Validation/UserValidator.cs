using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;
using System;

namespace Mock_Project_Net03.Validation
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Wrong Email Format");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Fullname can not be empty")
                .Matches("^[a-zA-Z\\s]*$")
                .WithMessage("Fullname can not be empty");
            // RuleFor(x => x.Password)
            //     .NotEmpty()
            //     .WithMessage("Password can not be empty");
            RuleFor(x => x.BirthDate)
                .NotNull().WithMessage("BirthDate can not be empty");
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender must be choice");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber can not be empty")
                .Matches("^0\\d{9,11}$").WithMessage("PhoneNumber must have 10-12 numbers and 0 as first");
        }
    }
    public static class UserValidatorExtension

    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this UserModel user)
        {
            var validator = new UserValidator();
            var result = await validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                throw new RequestValidationException(result.ToProblemDetails());
            }
            return null;
        }
    }
}
