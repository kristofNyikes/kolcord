using kolcordWebApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Channel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ChannelType Type { get; set; }
    public int ServerId { get; set; }
    public Server Server { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}