import RoundedImage from '../ImageComps/RoundedImage'
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
  return (
    <div className='flex items-center gap-2 bg-red-950/30 rounded-3xl m-3 w-4/5'>
      <RoundedImage src={friend.avatar} size={'11'}/>
      <span className=''>{friend.userName}</span>
    </div>
  )
}

export default FriendItem