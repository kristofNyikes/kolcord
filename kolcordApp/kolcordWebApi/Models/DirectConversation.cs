using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Models;

public class DirectConversation : Conversation
{
    public DirectConversation()
    {
        Type = ConversationType.Direct;
    }
}