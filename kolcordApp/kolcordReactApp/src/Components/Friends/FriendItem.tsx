import RoundedImage from '../ImageComps/RoundedImage'
import { FriendItemType } from '../../types/types'


const FriendItem = ({friend}: FriendItemType) => {
  return (
    <div className='flex items-center gap-2 bg-red-950/30 rounded-3xl m-3 w-4/5'>
      <RoundedImage src={friend.avatar} size={'11'}/>
      <span className=''>{friend.userName}</span>
    </div>
  )
}

export default FriendItem