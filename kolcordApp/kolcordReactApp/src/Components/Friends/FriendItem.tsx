import RoundedImage from "../ImageComps/RoundedImage";
import { FriendItemType } from "../../types/types";
import { useNavigate } from "react-router-dom";
import { fetchWithTokenCheck } from "../../Helpers/fetchWithTokenRefresh";
import { useState } from "react";

const FriendItem = ({ friend }: FriendItemType) => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleClick = async (e: React.MouseEvent) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      const response = await fetchWithTokenCheck(
        `/api/conversations/direct?targetUserId=${friend.id}`,
        { method: "POST" }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const conversation = await response.json();
      navigate(`/main/conversations/${conversation.id}`);
    } catch (error) {
      console.error("Error creating conversation:", error);
      alert("Failed to start conversation. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <button
      onClick={handleClick}
      disabled={isLoading}
      className="flex items-center gap-2 bg-red-950/30 hover:scale-105 transition-transform duration-300 m-3 w-4/5 rounded-3xl p-2"
    >
      <RoundedImage
        src={friend.avatar ? friend.avatar : "/user-image-backup.png"}
        size={"11"}
      />
      <span className="truncate">{friend.userName}</span>
      {isLoading && <span className="ml-2">...</span>}
    </button>
  );
};

export default FriendItem;
