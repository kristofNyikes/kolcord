using kolcordWebApi.Dtos.Conversation;
using kolcordWebApi.Models;

namespace kolcordWebApi.Mappers;

public static class ConversationMappers
{
    public static MessageDto FromMessageToDto(this Message message)
    {
        if (message == null) return null;

        return new MessageDto(
            message.Id,
            message.Content,
            message.TimeStamp,
            message.SenderId,
            message.Sender.UserName,
            message.ConversationId
        );
    }

    public static ParticipantDto FromParticipantToDto(this Participant participant)
    {
        if (participant == null) return null;

        return new ParticipantDto(
            participant.UserId,
            participant.User?.UserName ?? string.Empty
        );
    }

    public static ConversationDto FromConversationToDto(this Conversation conversation)
    {
        if (conversation == null) return null;

        // Safely handle Participants
        var participantDtos = conversation.Participants?
            .Where(p => p != null)
            .Select(p => p.FromParticipantToDto())
            .Where(p => p != null)
            .ToList() ?? new List<ParticipantDto>();

        // Safely get last message
        MessageDto lastMessageDto = null;
        if (conversation.Messages != null && conversation.Messages.Any())
        {
            var lastMessage = conversation.Messages
                .OrderByDescending(m => m.TimeStamp)
                .FirstOrDefault();

            lastMessageDto = lastMessage?.FromMessageToDto();
        }

        return new ConversationDto(
            conversation.Id,
            conversation.Name ?? string.Empty,
            conversation.Type,
            conversation.CreatedAt,
            participantDtos,
            lastMessageDto
        );
    }
}