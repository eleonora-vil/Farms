using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetUserInActiveRespone
    {
        public List<UserModel>? Users { get; set; }
        public string? messages { get; set; }
    }
}
