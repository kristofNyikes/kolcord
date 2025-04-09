using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Models;

public class Participant
{
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public ParticipantRole Role { get; set; }
}