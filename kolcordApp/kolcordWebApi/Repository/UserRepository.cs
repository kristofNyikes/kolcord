using kolcordWebApi.Data;
using kolcordWebApi.Dtos.User;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Mappers;
using kolcordWebApi.Models;
using kolcordWebApi.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kolcordWebApi.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
    {
        _userManager = userManager;
        _context = appDbContext;
    }
    public async Task<ApplicationUser?> GetByName(string name)
    {
        var user = await _userManager.FindByNameAsync(name);

        return user ?? null;
    }

    public async Task<List<UserWithFrStatus>?> SearchUsers(string name, string signedInUserId)
    {
        var users = await _context.Users
            .Where(u => u.UserName!.ToLower().Contains(name.ToLower()) && signedInUserId != u.Id)
            .Select(u => new
            {
                User = u,
                IsFriend = _context.Friendships.Any(f =>
                    (f.UserId == signedInUserId && f.FriendId == u.Id) ||
                    (f.UserId == u.Id && f.FriendId == signedInUserId))
            }).Take(15).ToListAsync();

        return users.Select(u => u.User.FromUserToUserWithFrStatus(u.IsFriend)).ToList();
    }
}