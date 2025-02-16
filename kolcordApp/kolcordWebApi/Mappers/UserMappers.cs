using kolcordWebApi.Dtos.User;
using kolcordWebApi.Models;

namespace kolcordWebApi.Mappers;

public static class UserMappers
{
    public static UserDto FromUserToUserDto(this ApplicationUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Avatar = user.Avatar,
            Bio = user.Bio,
            UserName = user.UserName!,
        };
    }

    public static FriendshipDto FromFriendshipToDto(this Friendship friendship)
    {
        return new FriendshipDto
        {
            Id = friendship.Id,
            UserId = friendship.UserId,
            FriendId = friendship.FriendId,
            FriendDto = friendship.Friend.FromUserToUserDto()
        };
    }

    public static FriendRequestDto FromFriendRequestToDto(this FriendRequest friendRequest)
    {
        return new FriendRequestDto
        {
            Id = friendRequest.Id,
            Sender = friendRequest.Sender.FromUserToUserDto(),
            FriendRequestStatus = friendRequest.FriendRequestStatus,
            CreatedAt = friendRequest.CreatedAt
        };
    }

    public static UserWithFrStatus FromUserToUserWithFrStatus(this ApplicationUser appUser, bool isFriendUser)
    {
        return new UserWithFrStatus
        {
            Id = appUser.Id,
            Avatar = appUser.Avatar,
            Bio = appUser.Bio,
            isFriend = isFriendUser,
            UserName = appUser.UserName!
        };
    }
}