namespace Mock_Project_Net03.Common.Payloads.Requests;

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}