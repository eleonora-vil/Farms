using Mock_Project_Net03.Dtos;

namespace Mock_Project_Net03.Common.Payloads.Responses;

public class GetUsersResponse
{
    public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();
}
