import React from 'react';
import { SearchResultItemType } from '../../types/types';
import RoundedImage from '../ImageComps/RoundedImage';
import { fetchWithTokenCheck } from '../../Helpers/fetchWithTokenRefresh';

const SearchResultItem: React.FC<SearchResultItemType> = ({ result }) => {
  const options = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
  }
  const onAddFriend = async () => {
    const response = await fetchWithTokenCheck(`/api/friendship/send-friend-request?receiverName=${result.userName}`, options);
    if(response.ok) {
      console.log('ok');
    }
  }

  return (
    <div className="flex items-center justify-between gap-2 bg-red-950/30 rounded-3xl m-3 w-4/5">
      <div className="flex items-center">
        <RoundedImage src={result.avatar} size={'11'} />
        <span className="">{result.userName}</span>
      </div>
      <div className="flex items-center gap-2 mr-2">
        {
          !result.isFriend && <button onClick={onAddFriend}>
          <img src="/add-friend-icon.png" alt="send friend request icon" className="w-8" />
        </button>
        }
        
      </div>
    </div>
  );
};

export default SearchResultItem;
