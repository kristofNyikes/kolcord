using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Models;

public abstract class Conversation
{
    public int Id { get; set; }
    public string Name { get; set; } //group/channel name
    public ConversationType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Message> Messages { get; set; }
    public ICollection<Participant> Participants { get; set; }
}