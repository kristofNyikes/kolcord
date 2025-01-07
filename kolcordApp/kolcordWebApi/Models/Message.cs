using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Message
{
    public int Id { get; set; }
    [Required]
    [StringLength(1500, ErrorMessage = "Content cannot be longer than 1500 characters")]
    public string Content { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}