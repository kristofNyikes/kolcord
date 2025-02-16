using kolcordWebApi.Dtos.User;
using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface IFriendshipRepository
{
    public Task<ApplicationUser?> GetByName(string name);
    public Task<List<Friendship>?> AddFriend(ApplicationUser user, string friendName);

    public Task<FriendRequest?> SendFriendRequest(ApplicationUser sender, string receiverName);
    public Task<bool> AcceptFriendRequest(int requestId, ApplicationUser user);
    public Task<bool> RejectFriendRequest(int requestId, ApplicationUser user);
    public Task<List<Friendship>?> GetFriendships(ApplicationUser user);
    public Task<List<FriendRequestDto>?> GetFriendRequests(ApplicationUser user);
}