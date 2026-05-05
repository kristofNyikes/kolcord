import { useEffect, useState } from "react";
import { fetchWithTokenCheck } from "../../Helpers/fetchWithTokenRefresh";
import Spinner from "../Spinner/Spinner";
import { useNavigate } from "react-router-dom";
import * as signalR from "@microsoft/signalr";
import { MessageDto, ConversationDto } from "../../types/types";

// export interface ConversationDto {
//   id: number;
//   name: string;
//   type: "Direct" | "Group" | "Channel";
//   createdAt: string;
//   participants: ParticipantDto[];
//   lastMessage?: MessageDto;
// }

const ConversationList = () => {
  const [conversations, setConversations] = useState<ConversationDto[]>([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const apiUrl = import.meta.env.VITE_BASE_URL;

  const sortConversations = (list: ConversationDto[]) => {
    return [...list].sort((a, b) => {
      const aTime = a.lastMessage
        ? new Date(a.lastMessage.timeStamp).getTime()
        : 0;
      const bTime = b.lastMessage
        ? new Date(b.lastMessage.timeStamp).getTime()
        : 0;
      return bTime - aTime;
    });
  };

  // --- fetch list on mount ---
  useEffect(() => {
    const load = async () => {
      const res = await fetchWithTokenCheck("/api/conversations", {});
      if (res.ok) {
        const data: ConversationDto[] = await res.json();

        // only show conversations that already have at least 1 message
        const withMessages = data.filter((c) => c.lastMessage != null);

        setConversations(sortConversations(withMessages));
      }
      setLoading(false);
    };

    load();
  }, []);

  // ----------- SignalR for realtime updates -----------
  useEffect(() => {
    const conn = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/conversationHub`, {
        accessTokenFactory: () => localStorage.getItem("accessToken") || "",
      })
      .withAutomaticReconnect()
      .build();

    conn
      .start()
      .then(() => {
        console.log("ConversationHub connected");

        // Listen for conversation updates
        conn.on("ConversationUpdated", (updated: ConversationDto) => {
          console.log("Conversation updated:", updated);
          setConversations((prev) => {
            const filtered = prev.filter((c) => c.id !== updated.id);
            if (!updated.lastMessage) return sortConversations(filtered);
            const newList = [updated, ...filtered];
            return sortConversations(newList);
          });
        });

        // Also listen for new messages to update conversations
        conn.on("ReceiveMessage", (message: MessageDto) => {
          // When a new message arrives, you might want to refetch conversations
          // or update the specific conversation
          console.log("New message received:", message);
          // You could trigger a refetch or update logic here
        });
      })
      .catch((err) => console.error("Hub error:", err));

    return () => {
      conn.off("ConversationUpdated");
      conn.off("ReceiveMessage");
      conn.stop();
    };
  }, [apiUrl]);

  // -------------------------------------------
  if (loading) return <Spinner />;

  if (conversations.length === 0)
    return <p className="text-center">No conversations yet</p>;

  return (
    <div className="flex flex-col items-center w-full">
      {conversations.map((c) => (
        <button
          key={c.id}
          onClick={() => navigate(`/main/conversations/${c.id}`)}
          className="flex items-center gap-3 bg-red-900/20 hover:bg-red-900/30 transition p-3 rounded-2xl w-4/5 mb-3"
        >
          <div className="flex flex-col text-left">
            <span className="font-bold">
              {c.name || c.participants.map((p) => p.userName).join(", ")}
            </span>

            {c.lastMessage && (
              <span className="text-sm opacity-60 truncate max-w-[200px]">
                {c.lastMessage.senderId === localStorage.getItem("userId")
                  ? `You: ${c.lastMessage.content}`
                  : `${c.lastMessage.senderName}: ${c.lastMessage.content}`}
              </span>
            )}
          </div>
        </button>
      ))}
    </div>
  );
};

export default ConversationList;
