using kolcordWebApi.Data;
using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Interfaces;
using kolcordWebApi.Mappers;
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
        // Fetch user data in one query
        var users = await _context.Users
            .Where(u => u.Id == userId1 || u.Id == userId2)
            .Select(u => new { u.Id, u.UserName })
            .ToDictionaryAsync(u => u.Id);

        if (users.Count != 2)
            throw new ArgumentException("One or both users not found");

        // Check for existing conversation using discriminator directly
        var existing = await _context.Conversations
            .Where(c => c.Type == ConversationType.Direct)
            .Where(c => c.Participants.Count(p => p.UserId == userId1 || p.UserId == userId2) == 2)
            .Select(c => new ConversationDto(
                c.Id,
                c.Name,
                c.Type,
                c.CreatedAt,
                c.Participants.Select(p => new ParticipantDto(
                    p.UserId,
                    users.ContainsKey(p.UserId) ? users[p.UserId].UserName : string.Empty
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

        // Create new conversation
        var conversation = new DirectConversation
        {
            Name = $"{users[userId1].UserName} & {users[userId2].UserName}",
            Participants = new List<Participant>
        {
            new() { UserId = userId1 },
            new() { UserId = userId2 }
        }
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        // Map to DTO after save
        return new ConversationDto(
            conversation.Id,
            conversation.Name,
            conversation.Type,
            conversation.CreatedAt,
            conversation.Participants
                .Select(p => new ParticipantDto(p.UserId, users[p.UserId].UserName))
                .ToList(),
            null // No messages yet
        );
    }

    public async Task<GroupConversation> CreateGroupConversation(string creatorId, string name, List<string> memberIds)
    {
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
        // Create and save the message
        var message = new Message
        {
            Content = content,
            ConversationId = conversationId,
            SenderId = senderId,
            TimeStamp = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // Fetch with complete projection
        return await _context.Messages
            .Where(m => m.Id == message.Id)
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

    public async Task<List<Message>> GetMessages(int conversationId, int skip = 0, int take = 10)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.TimeStamp)
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
}