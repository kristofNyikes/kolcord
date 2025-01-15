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

    //THIS IS FOR TESTING PURPOSES ONLY
    //NOT FOR PRODUCTION
    //-----------------------------------------------------
    [HttpPost("add-friend-instantly")]
    [Authorize]
    public async Task<IActionResult> AddFriend([FromQuery] string friendName)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("Sign in to use this function");
        }
        var friendShip = await _userRepo.AddFriend(user , friendName);
        if (friendShip == null)
        {
            return NotFound($"{friendName} could not be found or friendship already exists.");
        }

        var mappedFriendship = friendShip.Select(f => f.FromFriendshipToDto());
        return Ok(mappedFriendship);
    }
    //-----------------------------------------------------

    [HttpPost("send-friend-request")]
    [Authorize]
    public async Task<IActionResult> SendFriendRequest([FromQuery] string receiverName)
    {
        var sender = await _userManager.GetUserAsync(User);
        if (sender == null)
        {
            return Unauthorized("Sing in to send friend request");
        }

        var friendRequest = await _userRepo.SendFriendRequest(sender, receiverName);
        if (friendRequest == null)
        {
            return BadRequest($"{receiverName} could not be found or friendship already exists");
        }
        return Ok(friendRequest.FromFriendRequestToDto());
    }

    [HttpPost("accept-friend-request")]
    [Authorize]
    public async Task<IActionResult> AcceptFriendRequest([FromQuery] int requestId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("Sign in to accept friend request");
        }

        var success = await _userRepo.AcceptFriendRequest(requestId);
        if (!success)
        {
            return BadRequest("Friend request could not be accepted");
        }

        return Ok(new {Message = "Friendship accepted"});
    }

    [HttpPost("reject-friend-request")]
    [Authorize]
    public async Task<IActionResult> RejectFriendRequest([FromQuery] int requestId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("Sign in to reject friend request");
        }

        var success = await _userRepo.RejectFriendRequest(requestId);
        if (!success)
        {
            return NotFound("Friend request could not be rejected");
        }

        return Ok(new { Message = "Friendship rejected" });
    }

    [HttpGet("friend-list")]
    [Authorize]
    public async Task<IActionResult> GetFriends()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("Sign in to reject friend request");
        }

        var friends = await _userRepo.GetFriendships(user);

        var friendsDto = friends!.Select(f => f.FromFriendshipToDto());
        return Ok(friendsDto);
    }

}