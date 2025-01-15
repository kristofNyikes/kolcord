namespace kolcordWebApi.Dtos.User;

public class FriendRequestDto
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}