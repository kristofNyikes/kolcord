﻿namespace kolcordWebApi.Models;

public class Friendship
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string FriendId { get; set; }
    public ApplicationUser Friend { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}