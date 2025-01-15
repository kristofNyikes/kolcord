using kolcordWebApi.Data;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Models;
using kolcordWebApi.Models.Enums;
using Microsoft.AspNetCore.Identity;

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

    public async Task<List<Friendship>?> AddFriend(ApplicationUser user, string friendName)
    {
        var friend = await GetByName(friendName);
        if (friend == null)
        {
            return null;
        }

        var friendships = new List<Friendship>
        {
            new Friendship
            {
                UserId = user.Id,
                FriendId = friend.Id,
                CreatedAt = DateTime.UtcNow,
            },
            new Friendship
            {
                UserId = friend.Id,
                FriendId = user.Id,
                CreatedAt = DateTime.UtcNow,
            },
        };
        await _context.Friendships.AddRangeAsync(friendships);
        await _context.SaveChangesAsync();
        return friendships;
    }

    public async Task<FriendRequest?> SendFriendRequest(ApplicationUser sender, string receiverName)
    {
        var receiver = await GetByName(receiverName);
        if (receiver == null)
        {
            return null;
        }

        var friendRequest = new FriendRequest
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            FriendRequestStatus = FriendRequestStatus.Pending,
        };
        await _context.FriendRequests.AddAsync(friendRequest);
        await _context.SaveChangesAsync();
        return friendRequest;
    }

    public async Task<bool> AcceptFriendRequest(int requestId)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
        if (request == null || request.FriendRequestStatus != FriendRequestStatus.Pending)
        {
            return false;
        }

        request.FriendRequestStatus = FriendRequestStatus.Accepted;
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(request.SenderId);
        var receiver = await _context.Users.FindAsync(request.ReceiverId);
        if (sender != null && receiver != null)
        {
            await AddFriend(sender, receiver.UserName!);
        }

        return true;
    }

    public async Task<bool> RejectFriendRequest(int requestId)
    {
        return true;
    }
}