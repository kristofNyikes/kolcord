using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace kolcordWebApi.Models;

public class ApplicationUser : IdentityUser
{
    public string Avatar { get; set; } =
        "https://res.cloudinary.com/dilctmmuh/image/upload/v1736240885/riixinvcxidpygnqdexg.png";
    [StringLength(500, ErrorMessage = "Bio length cannot be more then 500 characters")]
    public string Bio { get; set; } = string.Empty;

    public string NickName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Gender { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<ServerMember> ServerMemberships { get; set; } = new List<ServerMember>();

    public ICollection<Server> OwnedServers { get; set; } = new List<Server>();
    public ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}