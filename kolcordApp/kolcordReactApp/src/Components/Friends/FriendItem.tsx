
type FriendDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
}

type FriendItem = {
  friend: FriendDto
}

const FriendItem = ({friend}: FriendItem) => {
  console.log(friend)
  return (
    <div>{friend.userName}</div>
  )
}

export default FriendItem