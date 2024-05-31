using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project_Net03.Common;
using Mock_Project_Net03.Common.Payloads.Requests;
using Mock_Project_Net03.Common.Payloads.Responses;
using Mock_Project_Net03.Dtos;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Exceptions;
using Mock_Project_Net03.Services;
using Mock_Project_Net03.Validation;

namespace Mock_Project_Net03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionService _permissionService;

        public PermissionController(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetListPermissions()
        {
            var result = await _permissionService.GetAllPermissions();
            return Ok(ApiResult<GetPermissionsRespone>.Succeed(new GetPermissionsRespone
            {
                Permissions = result,
            }));
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdatePermission([FromBody] List<PermissionRequest> permissions)
        {
            if (permissions == null)
            {
                return BadRequest("Permission list is null or empty");
            }
            var permissionModels = new List<PermissionModel>();



            var validator = new PermissionValidator();
            var errorResult = new PermissionErrorResult { Errors = new Dictionary<string, Dictionary<string, List<string>>> () };

            foreach (var permission in permissions)
            {
                var permissionModel = permission.ToPermissionModel();

                var validationResult = await validator.ValidateAsync(permissionModel);
                var permissionId = permissionModel.PermissionId;

                if (!validationResult.IsValid)
                {
                    // Collect error messages for each permission
                    var permissionErrors = new Dictionary<string, List<string>>();

                    foreach (var error in validationResult.Errors)
                    {
                        var propertyName = error.PropertyName;
                        if (!permissionErrors.ContainsKey(propertyName))
                        {
                            permissionErrors[propertyName] = new List<string>();
                        }
                        permissionErrors[propertyName].Add(error.ErrorMessage);
                    }

                    errorResult.Errors[$"PermissionId: {permissionId}"] = permissionErrors;
                }
                else
                {
                    permissionModels.Add(permissionModel);
                }
            }

            if (errorResult.Errors.Any())
            {
                // If there are any validation errors, return BadRequest with error result
                return BadRequest(errorResult);
            }

            if (errorResult.Errors.Any())
            {
                // If there are any validation errors, return BadRequest with error result
                return BadRequest(errorResult);
            }
            //PermissionValidator validation = new PermissionValidator();
            //var valid = await validation.ValidateAsync(permissionModels);
            //if (!valid.IsValid)
            //{
            //    throw new RequestValidationException(valid.ToProblemDetails());
            //}

            var result = await _permissionService.UpdatePermissions(permissionModels);
            if (result is not null)
            {
                return Ok(ApiResult<PermissionRespone>.Succeed(new PermissionRespone
                {
                    Permissions = permissionModels
                }));
            }
            return BadRequest();
        }
    }
}
