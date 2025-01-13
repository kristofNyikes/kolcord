using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}