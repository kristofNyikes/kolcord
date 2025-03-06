using kolcordWebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Hubs;

[Authorize]
public class FriendListHub: Hub
{
    public override async Task OnConnectedAsync()
    {
        //Console.WriteLine($"Connection established: {Context.ConnectionId}");
        //Console.WriteLine($"signalR hub on connect user id: {Context.UserIdentifier}");
        await base.OnConnectedAsync();
        //await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
    }
    public async Task AcceptedFriendRequest(string userId, FriendshipDto friendship)
    {
        await Clients.User(userId).SendAsync("NewFriendship", friendship);
    }
}