using kolcordWebApi.Hubs;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Mappers;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Controllers;

[Route("api/friendship")]
[ApiController]
public class FriendshipController : ControllerBase
{
    private readonly IFriendshipRepository _friendshipRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHubContext<FriendRequestsHub> _friendRequestHubContext;
    private readonly IHubContext<FriendListHub> _friendListHubContext;

    public FriendshipController(IFriendshipRepository friendshipRepo, UserManager<ApplicationUser> userManager, IHubContext<FriendRequestsHub> friendRequestHubContext, IHubContext<FriendListHub> friendListHubContext)
    {
        _friendshipRepo = friendshipRepo;
        _userManager = userManager;
        _friendRequestHubContext = friendRequestHubContext;
        _friendListHubContext = friendListHubContext;
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
        var friendShip = await _friendshipRepo.AddFriend(user, friendName);
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
            return Unauthorized("Sign in to send friend request");
        }

        var friendRequest = await _friendshipRepo.SendFriendRequest(sender, receiverName);
        if (friendRequest == null)
        {
            return BadRequest($"{receiverName} could not be found or friendship already exists");
        }

        var friendRequestDto = friendRequest.FromFriendRequestToDto();

        await _friendRequestHubContext.Clients.User(friendRequest.ReceiverId).SendAsync("NotifyNewFriendRequest", friendRequestDto);



        return Ok(friendRequestDto);
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

        var success = await _friendshipRepo.AcceptFriendRequest(requestId, user);
        if (!success)
        {
            return BadRequest("Friend request could not be accepted");
        }

        var friendRequest = await _friendshipRepo.GetFriendRequestById(requestId);

        if (friendRequest == null)
        {
            return BadRequest("Error getting friend request");
        }

        var newestFriendship = await _friendshipRepo.GetNewestFriendship(user);

        if (newestFriendship == null)
        {
            return BadRequest("Error getting friend list");
        }

        await _friendListHubContext.Clients.User(friendRequest.SenderId).SendAsync("NewFriendship", newestFriendship);

        return Ok(new { Message = "Friendship accepted" });
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

        var success = await _friendshipRepo.RejectFriendRequest(requestId, user);
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

        var friends = await _friendshipRepo.GetFriendships(user);

        var friendsDto = friends!.Select(f => f.FromFriendshipToDto());
        return Ok(friendsDto);
    }

    [HttpGet("friend-requests")]
    [Authorize]
    public async Task<IActionResult> GetFriendRequests()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("Sign in to get the friend requests");
        }

        var friendRequests = await _friendshipRepo.GetFriendRequests(user);
        return Ok(friendRequests);
    }
}