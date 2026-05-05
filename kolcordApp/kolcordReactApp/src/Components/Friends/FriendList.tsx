import { useEffect, useState } from "react";
import { fetchWithTokenCheck } from "../../Helpers/fetchWithTokenRefresh";
import Spinner from "../Spinner/Spinner";
import FriendItem from "./FriendItem";
import { Friend } from "../../types/types";
import * as signalR from "@microsoft/signalr";

const FriendList = ({ refreshTrigger }: { refreshTrigger?: number }) => {
  const [friendList, setFriendList] = useState<Friend[] | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const apiUrl = import.meta.env.VITE_BASE_URL;

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/friendListHub`, {
        accessTokenFactory: async () =>
          localStorage.getItem("accessToken") || "",
      })
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      connection.on("NewFriendship", (newFriend) => {
        setFriendList((prev) => {
          if (!prev) return [newFriend];
          if (prev.some((f) => f.id === newFriend.id)) return prev;

          return [...prev, newFriend];
        });
      });
    });

    return () => {
      connection.off("NewFriendship");
      connection.stop();
    };
    //apiUrl is constant from .env
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [refreshTrigger]);

  useEffect(() => {
    const fetchFriends = async () => {
      const response = await fetchWithTokenCheck(
        "/api/friendship/friend-list",
        {}
      );
      setIsLoading(true);
      if (response.ok) {
        const data = await response.json();
        setFriendList(data);
        setIsLoading(false);
      }
    };
    fetchFriends();
  }, [refreshTrigger]);

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
