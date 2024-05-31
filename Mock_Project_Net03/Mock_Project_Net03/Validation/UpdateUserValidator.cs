using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;

namespace Mock_Project_Net03.Validation
{
    public class UpdateUserValidator: AbstractValidator<UserModel>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Wrong Email Format");

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches("^[a-zA-Z\\s]*$")
                .When(x => !string.IsNullOrEmpty(x.FullName))
                .WithMessage("Wrong FullName Format");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches("^0\\d{9,11}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("PhoneNumber must have 10-12 numbers and 0 as first");
    
        }
    }
    public static class UpdateUserValidatorExtension

    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this UserModel user)
        {
            var validator = new UpdateUserValidator();
            var result = await validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                throw new RequestValidationException(result.ToProblemDetails());
            }
            return null;
        }
    }
}
