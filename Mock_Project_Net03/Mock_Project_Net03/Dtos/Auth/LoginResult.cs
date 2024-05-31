using Microsoft.IdentityModel.Tokens;

namespace Mock_Project_Net03.Dtos.Auth;

public class LoginResult
{
    public bool Authenticated { get; set; }
    public SecurityToken? Token { get; set; }
}