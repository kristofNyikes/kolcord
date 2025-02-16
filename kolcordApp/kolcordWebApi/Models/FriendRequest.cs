using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Models;

public class FriendRequest
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public ApplicationUser Sender { get; set; }
    public string ReceiverId { get; set; }
    public ApplicationUser Receiver { get; set; }
    public FriendRequestStatus FriendRequestStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}