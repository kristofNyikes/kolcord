using kolcordWebApi.Models.Enums;
using kolcordWebApi.Models;

namespace kolcordWebApi.Dtos.User;

public class FriendRequestDto
{
    public int Id { get; set; }
    public UserDto Sender { get; set; }
    public FriendRequestStatus FriendRequestStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}