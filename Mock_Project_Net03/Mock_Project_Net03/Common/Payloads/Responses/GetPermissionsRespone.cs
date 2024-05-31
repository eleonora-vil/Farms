using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetPermissionsRespone
    {
        public IEnumerable<PermissionModel> Permissions { get; set; } = new List<PermissionModel>();
    }
}
