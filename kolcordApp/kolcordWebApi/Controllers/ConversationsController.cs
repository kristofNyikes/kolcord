using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Hubs;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Mappers;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace kolcordWebApi.Controllers;

[Route("api/conversations")]
[ApiController]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IMessageRepository _repo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHubContext<ConversationHub> _hubContext;

    public ConversationsController(IMessageRepository repo, UserManager<ApplicationUser> userManager, IHubContext<ConversationHub> hubContext)
    {
        _repo = repo;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetConversations()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var conversations = await _repo.GetUserConversation(user.Id);

        var conversationDto = conversations.Select(c => c.FromConversationToDto());

        return Ok(conversationDto);
    }

    [HttpPost("direct")]
    public async Task<IActionResult> CreateDirectChat([FromQuery] string targetUserId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var conversation = await _repo.GetOrCreateDirectConversation(user.Id, targetUserId);
        return Ok(conversation);
    }

    [HttpPost("group")]
    public async Task<IActionResult> CreateGroupChat([FromBody] CreateGroupRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var conversation = await _repo.CreateGroupConversation(user.Id, request.Name, request.MemberIds);

        return Ok(conversation);
    }

    [HttpGet("{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(int conversationId, [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        var messages = await _repo.GetMessages(conversationId, skip, take);
        return Ok(messages);
    }

    [HttpPost("{conversationId}/messages")]
    public async Task<IActionResult> SendMessage(
        int conversationId,
        [FromBody] SendMessageRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var message = await _repo.SendMessage(
            user.Id,
            conversationId,
            request.Content,
            request.ReplyToMessageId
        );

        var conversation = await _repo.GetConversation(conversationId);
        if (conversation == null) return NotFound("Conversation not found");

        var conversationDto = conversation.FromConversationToDto();

        // Send message to conversation group
        await _hubContext.Clients.Group($"conv-{conversationId}")
            .SendAsync("ReceiveMessage", message);

        // Send conversation update to all participants
        var participantIds = conversationDto.Participants.Select(p => p.UserId.ToString()).ToList();
        Console.WriteLine($"Sending ConversationUpdated to users: {string.Join(", ", participantIds)}");

        await _hubContext.Clients.Users(participantIds)
            .SendAsync("ConversationUpdated", conversationDto);

        return Ok(message);
    }

    [HttpPost("{messageId}/mark-read")]
    public async Task<IActionResult> MarkMessageRead(int messageId)
    {
        var message = await _repo.GetMessage(messageId);
        if (message == null) return NotFound();

        await _hubContext.Clients.Group($"conv-{message.ConversationId}")
            .SendAsync("MessageRead", messageId);

        return Ok();
    }

    [HttpGet("last-message/{friendUserId}")]
    public async Task<IActionResult> GetLastMessage(string friendUserId)
    {
        var user = await _userManager.GetUserAsync(User);
        var conversation = await _repo.GetOrCreateDirectConversation(user.Id, friendUserId);
        var messageList = await _repo.GetMessages(conversation.Id, 0, 1);
        var lastMessage = messageList.Select(m => m.FromMessageToDto()).FirstOrDefault();

        return Ok(lastMessage);
    }

}