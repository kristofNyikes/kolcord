import React from 'react'
import FriendRequestItem from './FriendRequestItem';
import { useFriendRequests } from '../../Routes/FriendRequestsContext';

const FriendRequests: React.FC = () => {
  const { friendRequests, refetchFriendRequests } = useFriendRequests();

  const removeRequest = () => {
    refetchFriendRequests();
  };

  return (
    <div className='flex flex-col items-center'>
      {friendRequests && friendRequests?.length > 0 ? 
        friendRequests.map(req => {
          return <FriendRequestItem key={req.id} request={req} removeRequest={removeRequest}/>
        })
      : <p>Nothing, sorry</p>}
    </div>
  )
}

export default FriendRequests