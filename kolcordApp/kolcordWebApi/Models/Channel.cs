using kolcordWebApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace kolcordWebApi.Models;

public class Channel : Conversation
{
    public Channel()
    {
        Type = ConversationType.Channel;
    }
    public int ServerId { get; set; }
    public Server Server { get; set; }
}