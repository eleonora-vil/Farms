namespace Mock_Project_Net03.Common.Payloads.Requests;

public class SignupRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string fullname { get; set; } = null!;
    public string username { get; set; } = null!;
    public string gender { get; set; } = null!;
    public string level { get; set; } = null!;
    public string address { get; set; } = null!;
    public string fsu { get; set; } = null!;
    public string phone { get; set; } = null!;
}