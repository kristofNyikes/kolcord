import { useEffect, useState } from 'react';
import FriendItem from './FriendItem';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';

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

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck('/api/friendship/friend-list', {});
      if (response.ok) {
        const data = await response.json();
        setFriendList(data);
      }
    };
    fetchFriends();
  }, []);

  return (
    <div>
      {friendList && friendList.length > 0
        ? friendList.map((f) => {
            return <FriendItem friend={f.friendDto} key={f.id} />;
          })
        : 'no friends lol'}
    </div>
  );
};

export default FriendList;
