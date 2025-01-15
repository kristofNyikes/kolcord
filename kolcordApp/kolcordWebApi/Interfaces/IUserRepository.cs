using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface IUserRepository
{
    public Task<ApplicationUser?> GetByName(string name);
}