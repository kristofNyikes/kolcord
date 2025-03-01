import { useEffect, useState } from 'react';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';
import Spinner from '../Spinner/Spinner';
import FriendItem from './FriendItem';

type Friend = {
  id: number;
  userId: string;
  friendDto: FriendDto;
};
type FriendDto = {
  id: string;
  avatar: string;
  bio: string;
  userName: string;
};

const FriendList = () => {
  const [friendList, setFriendList] = useState<Friend[] | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck('/api/friendship/friend-list', {});
      setIsLoading(true);
      if (response.ok) {
        const data = await response.json();
        setFriendList(data);
        setIsLoading(false);
      }
    };
    fetchFriends();
  }, []);

  return (
    <div className='flex flex-col items-center'>
      {friendList && friendList.length > 0
        ? friendList.map((f) => {
            return <FriendItem friend={f.friendDto} key={f.id} />;
          })
        : isLoading ? <Spinner/> : <p>No friends</p>}
    </div>
  );
};

export default FriendList;
