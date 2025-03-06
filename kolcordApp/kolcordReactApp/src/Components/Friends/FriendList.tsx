import { useEffect, useState } from 'react';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';
import Spinner from '../Spinner/Spinner';
import FriendItem from './FriendItem';
import { Friend } from '../../types/types';
import * as signalR from '@microsoft/signalr';

const FriendList = () => {
  const [friendList, setFriendList] = useState<Friend[] | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const apiUrl = import.meta.env.VITE_BASE_URL;

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/friendListHub`, {
        accessTokenFactory: async () => localStorage.getItem('accessToken') || '',
      })
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      connection.on('NewFriendship', (newFriend: Friend) => {
        setFriendList((prev) => {
          if (prev && prev.some((f) => f.id === newFriend.id)) {
            console.log(prev);
            return prev;
          }
          console.log(prev);
          console.log(newFriend);
          return prev ? [...prev, newFriend] : [newFriend];
        });
      });
    });

    return () => {
      connection.off('NewFriendship');
      connection.stop();
    };
  }, [apiUrl]);

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
    <div className="flex flex-col items-center">
      {friendList && friendList.length > 0 ? (
        friendList.map((f) => {
          return <FriendItem friend={f.friendDto} key={f.id} />;
        })
      ) : isLoading ? (
        <Spinner />
      ) : (
        <p>No friends</p>
      )}
    </div>
  );
};

export default FriendList;
