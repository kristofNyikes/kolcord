using kolcordWebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Hubs;

[Authorize]
public class FriendRequestsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    public async Task SendFriendRequest(string userId, string friendId)
    {
        await Clients.User(userId).SendAsync("ReceiveFriendRequest", friendId);
    }

    public async Task NotifyNewFriendRequest(string userId, FriendRequestDto friendRequest)
    {
        await Clients.User(userId).SendAsync("NotifyNewFriendRequest", friendRequest);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}