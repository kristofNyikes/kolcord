/* eslint-disable react-refresh/only-export-components */
import React, { createContext, useContext, useEffect, useState } from 'react';
import { FriendRequestsContextType, Requests } from '../types/types';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';


const FriendRequestsContext = createContext<FriendRequestsContextType | null>(null);

export const useFriendRequests = () => {
  const context = useContext(FriendRequestsContext);
  if (!context) {
    throw new Error('useFriendRequests must be used within a FriendRequestsProvider');
  }
  return context;
};

export const FriendRequestsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [friendRequests, setFriendRequests] = useState<Requests[] | null>(null);

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