using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Exceptions;

namespace Mock_Project_Net03.Validation
{
    public class FileImportValidator:AbstractValidator<IFormFile>
    {
        public FileImportValidator()
        {
            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                                                         || x.Equals("application/vnd.ms-excel"))
                .WithMessage("File type '.xlsx / .xlsm / .xlsb / .xlsx' are required");
        }
    }
    public static class FileImportValidatorExtension 
    {
        public static async Task<ValidationProblemDetails> ValidateAsync(this IFormFile formFile) 
        {
            var validator = new FileImportValidator();
            var result = await validator.ValidateAsync(formFile);
            if (!result.IsValid)
            {
                throw new RequestValidationException(result.ToProblemDetails());
            }
            return null;
        } 
    }
}
