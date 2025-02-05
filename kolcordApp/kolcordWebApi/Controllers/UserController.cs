using kolcordWebApi.Dtos.User;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Mappers;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace kolcordWebApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IUserRepository userRepo, UserManager<ApplicationUser> userManager)
    {
        _userRepo  = userRepo;
        _userManager = userManager;
    }

    [HttpGet("get-user/{userName}")]
    [Authorize]
    public async Task<IActionResult> GetUser([FromRoute] string userName)
    {
        var user = await _userRepo.GetByName(userName);
        if (user == null)
        {
            return NotFound($"Can't find a user called: {userName}");
        }
        return Ok(user.FromUserToUserDto());
    }

    

}