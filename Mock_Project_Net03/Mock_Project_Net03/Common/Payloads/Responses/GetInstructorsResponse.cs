using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class GetInstructorsResponse
    {
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public string? message { get; set; }
    }
}
