namespace kolcordWebApi.Dtos.User;

public class FriendshipDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FriendId { get; set; }
    public UserDto FriendDto { get; set; }
}