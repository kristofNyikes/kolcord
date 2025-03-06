using kolcordWebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Hubs;

[Authorize]
public class FriendRequestsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        //Console.WriteLine($"Connection established: {Context.ConnectionId}");
        //Console.WriteLine($"signalR hub on connect user id: {Context.UserIdentifier}");
        await base.OnConnectedAsync();
        //await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
    }
    public async Task SendFriendRequest(string userId, string friendId)
    {
        await Clients.User(userId).SendAsync("ReceiveFriendRequest", friendId);
    }

    public async Task NotifyNewFriendRequest(string userId, FriendRequestDto friendRequest)
    {
        await Clients.User(userId).SendAsync("NotifyNewFriendRequest", friendRequest);
    }

    
}