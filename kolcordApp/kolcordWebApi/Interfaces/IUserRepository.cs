using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface IUserRepository
{
    public Task<ApplicationUser?> GetByName(string name);
    public Task<List<Friendship>?> AddFriend(ApplicationUser user, string friendName);

    public Task<FriendRequest?> SendFriendRequest(ApplicationUser sender, string receiverName);
    public Task<bool> AcceptFriendRequest(int requestId);
    public Task<bool> RejectFriendRequest(int requestId);
    public Task<List<Friendship>?> GetFriendships(ApplicationUser user);
}