import { useEffect, useState } from 'react'
import FriendItem from './FriendItem'
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh'

type Friend = {
  id: number;
  userId: string;
  friendDto: FriendDto;
  
}
type FriendDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
}

const FriendList = () => {
  const [friendList, setFriendList] = useState<Friend[] | null>(null);

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck("/api/friendship/friend-list", {});
      if(response.ok) {
        const data = await response.json();
        console.log(data)
        console.log(data[0])
        console.log(data[0].friendDto)
        setFriendList(data);
      }
    }
    fetchFriends();
  }, [])

  return (
    <div>
      {friendList ? friendList.map(f => {
        return <FriendItem friend={f.friendDto} key={f.id}/>
      }) : "placeholder"
    
      }
    </div>
    // <div></div>
  )
}

export default FriendList