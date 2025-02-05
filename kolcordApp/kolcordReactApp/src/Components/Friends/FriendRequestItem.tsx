import React from 'react';
import RoundedImage from '../ImageComps/RoundedImage';
import { FriendRequestItemProp } from '../../types/types';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';

const FriendRequestItem: React.FC<FriendRequestItemProp> = ({ request, removeRequest }) => {


  const options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
  }

  const onAccept = async (): Promise<void> => {
    try {
      const response = await fetchWithTokenCheck(`/api/friendship/accept-friend-request?requestId=${request.id}`, options);
      if(response.ok) {
        removeRequest();
      }
    } catch (error) {
      console.error(error);
    }
  }

  const onReject = async (): Promise<void> => {
    try {
      const response = await fetchWithTokenCheck(`/api/friendship/reject-friend-request?requestId=${request.id}`, options);
      if(response.ok) {
        removeRequest();
      }
    } catch (error) {
      console.error(error);
    }
  }


  return (
    <div className="flex items-center justify-between gap-2 bg-red-950/30 rounded-3xl m-3 w-4/5">
      <div className='flex items-center'>
        <RoundedImage src={request.sender.avatar} size={'11'} />
        <span className="">{request.sender.userName}</span>
      </div>
      <div className='flex items-center gap-2 mr-2'>
        <button onClick={onAccept}>
          <img src="/accept-icon.png" alt="accept friend request icon" className='w-5'/>
        </button>
        <button onClick={onReject}>
          <img src="/reject-icon.png" alt="reject friend request icon" className='w-5'/>
        </button>
      </div>
    </div>
  );
};

export default FriendRequestItem;
