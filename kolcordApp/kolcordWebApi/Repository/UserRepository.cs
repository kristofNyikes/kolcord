using kolcordWebApi.Data;
using kolcordWebApi.Interfaces;
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
}