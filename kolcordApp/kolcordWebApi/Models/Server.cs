using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Server
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Server name is required")]
    [MinLength(3, ErrorMessage = "Server name cannot be shorter than 3 characters")]
    [StringLength(40, ErrorMessage = "Server name cannot be longer than 40 characters")]
    public string Name { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerMember> Members { get; set; }
    public ICollection<Channel> Channels { get; set; }
}