using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;

namespace Mock_Project_Net03.Validation
{
    public class ClassValidator : AbstractValidator<ClassModel>
    {
        public ClassValidator()
        {
            RuleFor(x => x.ClassName)
                .NotEmpty().WithMessage("ClassName can not be empty")
                .MaximumLength(255).WithMessage("ClassName can not be longer than 255 characters");
            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage("SemesterId can not be empty");
        }
    }
        public static class ClassValidatorExtension

        {
            public static async Task<ValidationProblemDetails> ValidateAsync(this ClassModel classModel)
            {
                var validator = new ClassValidator();
                var result = await validator.ValidateAsync(classModel);
                if (!result.IsValid)
                {
                    throw new RequestValidationException(result.ToProblemDetails());
                }
                return null;
            }
        }
}
