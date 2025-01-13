using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Dtos.Account;

public class LoginReq
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}