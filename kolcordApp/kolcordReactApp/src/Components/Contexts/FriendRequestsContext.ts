import { createContext, useContext } from 'react';
import { FriendRequestsContextType } from '../../types/types';

export const FriendRequestsContext = createContext<FriendRequestsContextType | null>(null);

export const useFriendRequests = () => {
  const context = useContext(FriendRequestsContext);
  if (!context) {
    throw new Error('useFriendRequests must be used within a FriendRequestsProvider');
  }
  return context;
};