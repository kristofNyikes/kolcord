import React, { useState } from "react";
import FriendRequestItem from "./FriendRequestItem";
import { useFriendRequests } from "../Contexts/FriendRequestsContext";
import FriendList from "./FriendList";

const FriendRequests: React.FC = () => {
  const { friendRequests, refetchFriendRequests } = useFriendRequests();
  const [refreshKey, setRefreshKey] = useState<number>(0);

  const removeRequest = () => {
    refetchFriendRequests();
  };

  const handleRequestProcessed = () => {
    setRefreshKey((prev) => prev + 1);
  };

  return (
    <>
      <div className="flex flex-col items-center">
        {friendRequests && friendRequests?.length > 0
          ? friendRequests.map((req) => {
              return (
                <FriendRequestItem
                  key={req.id}
                  request={req}
                  removeRequest={removeRequest}
                  onRequestProcessed={handleRequestProcessed}
                />
              );
            })
          : " "}
      </div>
      <div>
        <FriendList refreshTrigger={refreshKey}></FriendList>
      </div>
    </>
  );
};

export default FriendRequests;
