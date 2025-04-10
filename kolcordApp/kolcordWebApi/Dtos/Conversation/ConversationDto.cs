using kolcordWebApi.Models.Enums;

namespace kolcordWebApi.Dtos.Conversation;

public record ConversationDto(int Id, string Name, ConversationType Type, DateTime CreatedAt, List<ParticipantDto> Participants, MessageDto LastMessage);