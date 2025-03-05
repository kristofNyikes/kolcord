import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { Requests } from '../../types/types';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';
import { FriendRequestsContext } from './FriendRequestsContext';

export const FriendRequestsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [friendRequests, setFriendRequests] = useState<Requests[] | null>(null);
  const apiUrl = import.meta.env.VITE_BASE_URL;
  
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/friendRequestsHub`, {
        accessTokenFactory: () => localStorage.getItem('accessToken') || ''
      })
      .withAutomaticReconnect()
      .build();
  
    connection.start()
      .then(() => {
        connection.on('NotifyNewFriendRequest', (newFriendRequest: Requests) => {
          setFriendRequests((prevRequests) => 
            prevRequests ? [...prevRequests, newFriendRequest] : [newFriendRequest]
          );
        });
      })
      .catch(err => console.error('SignalR Connection Error: ', err));

    return () => {
      connection.stop();
    };
  }, [apiUrl]);

  const refetchFriendRequests = async () => {
    const response = await fetchWithTokenCheck('/api/friendship/friend-requests', {});
    if (response.ok) {
      const data = await response.json();
      setFriendRequests(data);
    }
  };

  useEffect(() => {
    refetchFriendRequests();
  }, []);

  return (
    <FriendRequestsContext.Provider value={{ friendRequests, refetchFriendRequests }}>
      {children}
    </FriendRequestsContext.Provider>
  );
};