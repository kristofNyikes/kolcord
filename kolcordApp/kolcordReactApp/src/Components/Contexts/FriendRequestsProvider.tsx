import React, { useEffect, useState } from "react";

import { Requests } from "../../types/types";
import { fetchWithTokenCheck } from "../../Helpers/fetchWithTokenRefresh";
import { FriendRequestsContext } from "./FriendRequestsContext";
import * as signalR from "@microsoft/signalr";

export const FriendRequestsProvider: React.FC<{
  children: React.ReactNode;
}> = ({ children }) => {
  const [friendRequests, setFriendRequests] = useState<Requests[] | null>(null);
  const apiUrl = import.meta.env.VITE_BASE_URL;

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/friendRequestsHub`, {
        accessTokenFactory: async () =>
          localStorage.getItem("accessToken") || "",
      })
      .configureLogging("warning")
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      connection.on("NotifyNewFriendRequest", (newFriendRequest: Requests) => {
        setFriendRequests((prev) =>
          prev ? [...prev, newFriendRequest] : [newFriendRequest],
        );
      });
    });

    return () => {
      connection.off("NotifyNewFriendRequest");
      connection.stop();
    };
    //apiUrl is constant from .env
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const refetchFriendRequests = async () => {
    const response = await fetchWithTokenCheck(
      "/api/friendship/friend-requests",
      {},
    );
    if (response.ok) {
      const data = await response.json();
      setFriendRequests(data);
    }
  };

  useEffect(() => {
    refetchFriendRequests();
  }, []);

  return (
    <FriendRequestsContext.Provider
      value={{ friendRequests, refetchFriendRequests }}
    >
      {children}
    </FriendRequestsContext.Provider>
  );
};
