using kolcordWebApi.Data;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Models.Enums;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kolcordWebApi.Repository;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public FriendshipRepository(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
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
        var exists = await _context.Friendships.AnyAsync(f => (f.UserId == user.Id && f.FriendId == friend.Id) || (f.UserId == friend.Id && f.FriendId == user.Id));

        if (exists) return null;

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

        var exists = await _context.Friendships.AnyAsync(f => (f.UserId == sender.Id && f.FriendId == receiver.Id) || (f.UserId == receiver.Id && f.FriendId == sender.Id));

        if (exists) return null;

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
            var result = await AddFriend(sender, receiver.UserName!);

            if (result == null)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> RejectFriendRequest(int requestId)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
        if (request == null || request.FriendRequestStatus != FriendRequestStatus.Pending)
        {
            return false;
        }

        request.FriendRequestStatus = FriendRequestStatus.Rejected;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Friendship>?> GetFriendships(ApplicationUser user)
    {
        var friends = await _context.Friendships.Where(f => f.UserId == user.Id).Include(f => f.Friend).ToListAsync();
        return friends;
    }
}