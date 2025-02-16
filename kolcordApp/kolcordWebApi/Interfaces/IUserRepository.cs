using kolcordWebApi.Dtos.User;
using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface IUserRepository
{
    public Task<ApplicationUser?> GetByName(string name);
    public Task<List<UserWithFrStatus>?> SearchUsers(string name, string signedInUserId);
}