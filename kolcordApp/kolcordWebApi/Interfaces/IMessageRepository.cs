using kolcordWebApi.Models;

namespace kolcordWebApi.Interfaces;

public interface IMessageRepository
{
    public Task<Conversation> GetOrCreateDirectConversation(string userId1, string userId2);
    public Task<GroupConversation> CreateGroupConversation(string creatorId, string name, List<string> memberIds);
    public Task<Message> SendMessage(string senderId, int conversationId, string content);
    public Task<List<Message>> GetMessages(int conversationId, int skip = 0, int take = 10);
    public Task<List<Conversation>> GetUserConversation(string userId);
}