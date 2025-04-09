using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Models;

public class GroupConversation : Conversation
{
    public GroupConversation()
    {
        Type = ConversationType.Group;
    }
}