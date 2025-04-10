//using kolcordWebApi.Dtos.Conversation;
//using kolcordWebApi.Models;

//namespace kolcordWebApi.Mappers;

//public static class ConversationMappers
//{
//    public static MessageDto FromMessageToDto(this Message message)
//    {
//        return new MessageDto(message.Content, message.TimeStamp.Date);
//    }

//    public static ParticipantDto FromParticipantToDto(this Participant participant)
//    {
//        return new ParticipantDto(participant.UserId, participant.User.UserName!);
//    }

//    public static ConversationDto FromConversationToDto(this Conversation conversation)
//    {
//        return new ConversationDto(
//            conversation.Id,
//            conversation.Name,
//            conversation.Type,
//            conversation.CreatedAt,
//            conversation.Participants?
//                .Select(p => new ParticipantDto(p.UserId, p.User?.UserName ?? string.Empty))
//                .ToList() ?? new List<ParticipantDto>(),
//            conversation.Messages?
//                .OrderByDescending(m => m.TimeStamp)
//                .Select(m => new MessageDto(m.Content, m.TimeStamp))
//                .FirstOrDefault()
//        );
//    }


//}