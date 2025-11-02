namespace kolcordWebApi.Dtos.Conversation;

public record CreateGroupRequest(string Name, List<string> MemberIds);