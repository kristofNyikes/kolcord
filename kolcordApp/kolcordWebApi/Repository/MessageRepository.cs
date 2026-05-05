using kolcordWebApi.Data;
using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Models;
using kolcordWebApi.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kolcordWebApi.Repository;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessageRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ConversationDto> GetOrCreateDirectConversation(string userId1, string userId2)
    {
        var user1 = await _userManager.FindByIdAsync(userId1);
        var user2 = await _userManager.FindByIdAsync(userId2);

        if (user1 == null || user2 == null)
            throw new ArgumentException("One or both users not found");

        var existing = await _context.Conversations
            .OfType<DirectConversation>()
            .Include(c => c.Participants)
            .ThenInclude(p => p.User)
            .Include(c => c.Messages)
            .ThenInclude(m => m.Sender)
            .Where(c => c.Participants.Any(p => p.UserId == userId1) &&
                       c.Participants.Any(p => p.UserId == userId2))
            .Select(c => new ConversationDto(
                c.Id,
                c.Name,
                c.Type,
                c.CreatedAt,
                c.Participants.Select(p => new ParticipantDto(
                    p.UserId,
                    p.User.UserName
                )).ToList(),
                c.Messages
                    .OrderByDescending(m => m.TimeStamp)
                    .Select(m => new MessageDto(
                        m.Id,
                        m.Content,
                        m.TimeStamp,
                        m.SenderId,
                        m.Sender.UserName,
                        m.ConversationId
                    ))
                    .FirstOrDefault()
            ))
            .FirstOrDefaultAsync();

        if (existing != null)
            return existing;

        var conversation = new DirectConversation
        {
            Name = $"{user1.UserName} & {user2.UserName}",
            Participants = new List<Participant>
            {
                new() { UserId = userId1 },
                new() { UserId = userId2 }
            }
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        var createdConversation = await _context.Conversations
            .OfType<DirectConversation>()
            .Include(c => c.Participants)
            .ThenInclude(p => p.User)
            .FirstAsync(c => c.Id == conversation.Id);

        return new ConversationDto(
            createdConversation.Id,
            createdConversation.Name,
            createdConversation.Type,
            createdConversation.CreatedAt,
            createdConversation.Participants
                .Select(p => new ParticipantDto(p.UserId, p.User.UserName))
                .ToList(),
            null
        );
    }

    public async Task<GroupConversation> CreateGroupConversation(string creatorId, string name, List<string> memberIds)
    {
        var usersExist = await _context.Users
            .Where(u => memberIds.Contains(u.Id))
            .CountAsync() == memberIds.Count;

        if (!usersExist)
            throw new ArgumentException("One or more users not found");

        var conversation = new GroupConversation
        {
            Name = name,
            Participants = memberIds.Select(id => new Participant
            {
                UserId = id,
                Role = id == creatorId ? ParticipantRole.Owner : ParticipantRole.Member
            }).ToList()
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<MessageDto> SendMessage(string senderId, int conversationId, string content, int? replyToMessageId = null)
    {
        var message = new Message
        {
            Content = content,
            ConversationId = conversationId,
            SenderId = senderId,
            TimeStamp = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return await _context.Messages
            .Where(m => m.Id == message.Id)
            .Include(m => m.Sender)
            .Select(m => new MessageDto(
                m.Id,
                m.Content,
                m.TimeStamp,
                m.SenderId,
                m.Sender.UserName,
                m.ConversationId
            ))
            .FirstAsync();
    }

    public async Task<List<Message>> GetMessages(int conversationId, int skip = 0, int take = 20)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.TimeStamp)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<Conversation>> GetUserConversation(string userId)
    {
        return await _context.Conversations
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Include(c => c.Participants)
            .ThenInclude(p => p.User)
            .Include(c => c.Messages.OrderByDescending(m => m.TimeStamp).Take(1))
            .ToListAsync();
    }

    public async Task<Message?> GetMessage(int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null) return null;

        message.IsRead = true;
        await _context.SaveChangesAsync();
        return message;
    } 

    public async Task<Conversation?> GetConversation(int conversationId)
    {
        var conversation = await _context.Conversations.FindAsync(conversationId);
        if (conversation == null) return null;

        return conversation;
    }
}