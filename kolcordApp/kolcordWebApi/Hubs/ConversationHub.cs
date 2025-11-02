using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Hubs;

[Authorize]
public class ConversationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task SendTypingIndicator(int conversationId, bool isTyping)
    {
        await Clients.Groups($"conv-{conversationId}")
            .SendAsync("ReceiveTypingIndicator", Context.UserIdentifier, isTyping);
    }

    public async Task SendNewMessage(int conversationId, MessageDto message)
    {
        await Clients.Group($"conv-{conversationId}")
            .SendAsync("ReceiveMessage", message);
    }

    public async Task JoinConversationGroup(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv-{conversationId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}