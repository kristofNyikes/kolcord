using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace kolcordWebApi.Controllers;

[Route("api/conversations")]
[ApiController]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IMessageRepository _repo;
    private readonly UserManager<ApplicationUser> _userManager;

    public ConversationsController(IMessageRepository repo, UserManager<ApplicationUser> userManager)
    {
        _repo = repo;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetConversation()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var conversations = await _repo.GetUserConversation(user.Id);

        return Ok(conversations);
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
        [FromQuery] int take = 50)
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
        return Ok(message);
    }
}