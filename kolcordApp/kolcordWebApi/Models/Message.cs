using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Message
{
    public int Id { get; set; }
    [Required]
    [StringLength(1500, ErrorMessage = "Content cannot be longer than 1500 characters")]
    public string Content { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }
    public string SenderId { get; set; }
    public ApplicationUser Sender { get; set; }
}