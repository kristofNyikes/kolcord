
namespace kolcordWebApi.Dtos.Conversation;


public record MessageDto(
    int Id,
    string Content,
    DateTime TimeStamp,
    string SenderId,
    string SenderName,
    int ConversationId
);