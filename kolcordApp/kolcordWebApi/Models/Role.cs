using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Role
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "Role name cannot be longer than 50 characters")]
    public string Name { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
    public ICollection<ServerMember> Members { get; set; } = new List<ServerMember>();
}