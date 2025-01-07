namespace kolcordWebApi.Models;

public class ServerMember
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
    public string? Nickname { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}