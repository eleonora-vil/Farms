using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using System;

namespace Mock_Project_Net03.Validation
{
    public class PermissionValidator : AbstractValidator<PermissionModel>
    {
        public PermissionValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;
            RuleLevelCascadeMode = CascadeMode.Continue;
            RuleFor(x => x.SyllabusAccess)
                .NotEmpty()
                .WithMessage("Syllabus Access can not be null or empty")
                .Must(access =>
                {
                    return access == "Access denied" || access == "View" || access == "Modify" || access == "Create" || access == "Full access";
                }).WithMessage("Permission is wrong, check format string is true or not");
            //.Matches("Access denied")
            //.WithMessage("Permission is wrong, check format string is true or not")
            //.Matches("View")
            //.WithMessage("Permission is wrong, check format string is true or not")
            //.Matches("Modify")
            //.WithMessage("Permission is wrong, check format string is true or not")
            //.Matches("Create")
            //.WithMessage("Permission is wrong, check format string is true or not")
            //.Matches("Full access")
            //.WithMessage("Permission is wrong, check format string is true or not");
            RuleFor(x => x.ProgramAccess)
                .NotEmpty()
                .WithMessage("Program Access can not be null or empty")
                .Must(access =>
                {
                    return access == "Access denied" || access == "View" || access == "Modify" || access == "Create" || access == "Full access";
                }).WithMessage("Permission is wrong, check format string is true or not");
            RuleFor(x => x.UserAccess)
                .NotEmpty()
                .WithMessage("User Access can not be null or empty")
                .Must(access =>
                {
                    return access == "Access denied" || access == "View" || access == "Modify" || access == "Create" || access == "Full access";
                }).WithMessage("Permission is wrong, check format string is true or not");
            RuleFor(x => x.ClassAccess)
                .NotEmpty()
                .WithMessage("Class Access can not be null or empty")
                .Must(access =>
                {
                    return access == "Access denied" || access == "View" || access == "Modify" || access == "Create" || access == "Full access";
                }).WithMessage("Permission is wrong, check format string is true or not");
            RuleFor(x => x.MaterialAccess)
                .NotEmpty()
                .WithMessage("Material Access can not be null or empty")
                .Must(access =>
                {
                    return access == "Access denied" || access == "View" || access == "Modify" || access == "Create" || access == "Full access";
                }).WithMessage("Permission is wrong, check format string is true or not");
        }
    }
    public static class PermissionValidatorExtension

    {
        //public static async Task<List<ValidationProblemDetails>> ValidateAsync(this List<PermissionModel> permissions)
        //{
        //    var validator = new PermissionValidator();
        //    List<ValidationProblemDetails> problemDetails = new List<ValidationProblemDetails>();

        //    foreach (var permission in permissions)
        //    {
        //        var validationResult = await validator.ValidateAsync(permission);
        //        if (!validationResult.IsValid)
        //        {
        //            throw new RequestValidationException(validationResult.ToProblemDetails());
        //        }
        //    }
        //    return problemDetails;
        //}
        public static async Task<ValidationProblemDetails> ValidateAsync(this PermissionModel permissionModel)
        {
            var validator = new PermissionValidator();
            var result = await validator.ValidateAsync(permissionModel);
            if (!result.IsValid)
            {
                throw new RequestValidationException(result.ToProblemDetails());
            }
            return null;
        }
    }
}
