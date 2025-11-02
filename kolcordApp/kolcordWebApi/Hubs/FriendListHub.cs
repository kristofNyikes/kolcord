using kolcordWebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Hubs;

[Authorize]
public class FriendListHub: Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    public async Task AcceptedFriendRequest(string userId, FriendshipDto friendship)
    {
        await Clients.User(userId).SendAsync("NewFriendship", friendship);
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}