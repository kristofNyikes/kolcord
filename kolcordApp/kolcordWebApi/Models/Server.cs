using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Server
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Server name is required")]
    [MinLength(3, ErrorMessage = "Server name cannot be shorter than 3 characters")]
    [StringLength(40, ErrorMessage = "Server name cannot be longer than 40 characters")]
    public string Name { get; set; }
    [StringLength(1000, ErrorMessage = "Server description cannot be longer than 1000 characters")]
    public string Description { get; set; } = string.Empty;
    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}