using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Dtos.Conversation;

public record SendMessageRequest(
    [Required] string Content,
    int? ReplyToMessageId = null
);