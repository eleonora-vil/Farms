using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class CreateUsersRespone
    {
        public UserModel User { get; set; }

        public string? message { get; set; }
    }
}
