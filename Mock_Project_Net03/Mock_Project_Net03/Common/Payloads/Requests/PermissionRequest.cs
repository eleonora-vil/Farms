using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Requests
{
    public class PermissionRequest
    {
        public int PermissionId { get; set; }
        public string SyllabusAccess { get; set; }
        public string ProgramAccess { get; set; }
        public string UserAccess { get; set; }
        public string ClassAccess { get; set; }
        public string MaterialAccess { get; set; }
        public int RoleID { get; set; }
    }
    public static class PermissionRequestExt
    {
        public static PermissionModel ToPermissionModel(this PermissionRequest permissionRequest)
        {
            var PermissionModel = new PermissionModel()
            {
                PermissionId = permissionRequest.PermissionId,
                SyllabusAccess = permissionRequest.SyllabusAccess,
                ProgramAccess = permissionRequest.ProgramAccess,
                UserAccess = permissionRequest.UserAccess,
                ClassAccess = permissionRequest.ClassAccess,
                MaterialAccess = permissionRequest.MaterialAccess,
                RoleID = permissionRequest.RoleID,
            };
            return PermissionModel;
        }
    }
}
