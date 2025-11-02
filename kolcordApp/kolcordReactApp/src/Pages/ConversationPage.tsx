/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useRef, useState, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { fetchWithTokenCheck } from '../Helpers/fetchWithTokenRefresh';
import { MessageDto } from '../types/types';
import MessageInput from '../Components/Conversation/MessageInput';
import MessageList from '../Components/Conversation/MessageList';
import * as signalR from '@microsoft/signalr';

const MESSAGE_PAGE_SIZE = 20;
const SCROLL_LOAD_THRESHOLD = 100;
const DEBOUNCE_DELAY = 200;

function debounce<T extends (...args: any[]) => void>(fn: T, delay: number) {
  let timeoutId: any;
  return (...args: Parameters<T>) => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => fn(...args), delay);
  };
}

export default function ConversationPage() {
  const { conversationId } = useParams<{ conversationId: string }>();
  const [messages, setMessages] = useState<MessageDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [hasMore, setHasMore] = useState(true);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [optimisticMessages, setOptimisticMessages] = useState<MessageDto[]>([]);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const messagesContainerRef = useRef<HTMLDivElement>(null);
  const apiUrl = import.meta.env.VITE_BASE_URL;

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/conversationHub`, {
        accessTokenFactory: () => localStorage.getItem('accessToken') || '',
      })
      .withAutomaticReconnect()
      .build();

    const startConnection = async () => {
      try {
        await connection.start();
        await connection.invoke('JoinConversationGroup', parseInt(conversationId || '0'));

        connection.on('ReceiveMessage', (message: MessageDto) => {
          setMessages((prev) => {
            if (prev.some((m) => m.id === message.id)) {
              return prev;
            }
            return [...prev, message];
          });
        });
      } catch (err) {
        console.error('SignalR Connection Error:', err);
        setTimeout(startConnection, 5000);
      }
    };

    startConnection();

    return () => {
      connection.off('ReceiveMessage');
      connection.stop();
    };
  }, [apiUrl]);

  const loadMessages = useCallback(
    async (skip = 0) => {
      try {
        const response = await fetchWithTokenCheck(`/api/conversations/${conversationId}/messages?skip=${skip}&take=${MESSAGE_PAGE_SIZE}`, {});
        const newMessages = await response.json();
        return newMessages;
      } catch (error) {
        console.error('Error loading messages:', error);
        return [];
      }
    },
    [conversationId]
  );

  useEffect(() => {
    if (!conversationId) return;

    const loadInitialMessages = async () => {
      setIsLoading(true);
      const initialMessages = await loadMessages(0);
      setMessages(initialMessages);
      setHasMore(initialMessages.length === MESSAGE_PAGE_SIZE);
      setIsLoading(false);
    };

    loadInitialMessages();
  }, [conversationId, loadMessages]);

  useEffect(() => {
    if (!isLoading && messages.length > 0) {
      scrollToBottom();
    }
  }, [isLoading, messages.length]);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const isNearTop = (container: HTMLDivElement, threshold: number) => {
    return container.scrollTop <= threshold;
  };

  const handleScroll = useCallback(
    debounce(() => {
      const container = messagesContainerRef.current;
      if (!container || isLoadingMore || !hasMore) return;

      if (container.scrollTop <= SCROLL_LOAD_THRESHOLD + 50) {
        loadMoreMessages();
      }
    }, DEBOUNCE_DELAY),
    [isLoadingMore, hasMore, messages.length]
  );

  useEffect(() => {
    const container = messagesContainerRef.current;
    if (!container) return;

    container.addEventListener('scroll', handleScroll);
    return () => container.removeEventListener('scroll', handleScroll);
  }, [handleScroll]);

  const loadMoreMessages = async () => {
    if (isLoadingMore || !hasMore) return;

    setIsLoadingMore(true);
    const container = messagesContainerRef.current;

    const scrollHeightBefore = container?.scrollHeight || 0;
    const scrollTopBefore = container?.scrollTop || 0;
    const firstMessageIdBefore = messages[0]?.id;

    try {
      const olderMessages = await loadMessages(messages.length);
      if (olderMessages.length < MESSAGE_PAGE_SIZE) {
        setHasMore(false);
      }

      setMessages((prev) => [...olderMessages, ...prev]);

      requestAnimationFrame(() => {
        if (!container) return;

        const newFirstMessageId = olderMessages[0]?.id;
        if (firstMessageIdBefore && newFirstMessageId && firstMessageIdBefore !== newFirstMessageId) {
          const scrollHeightAfter = container.scrollHeight;
          container.scrollTop = scrollTopBefore + (scrollHeightAfter - scrollHeightBefore);
        }
      });
    } catch (error) {
      console.error('Error loading older messages:', error);
    } finally {
      setIsLoadingMore(false);
    }
  };
  const handleSendMessage = async (content: string) => {
    const tempId = -Date.now();
    const optimisticMessage: MessageDto = {
      id: tempId,
      content,
      timeStamp: new Date().toISOString(),
      senderId: 'current-user',
      senderName: 'You',
      conversationId: parseInt(conversationId || '0'),
    };

    setOptimisticMessages((prev) => [...prev, optimisticMessage]);

    try {
      const response = await fetchWithTokenCheck(`/api/conversations/${conversationId}/messages`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ content }),
      });

      setOptimisticMessages((prev) => prev.filter((m) => m.id !== tempId));
    } catch (error) {
      console.error('Error sending message:', error);
      setOptimisticMessages((prev) => prev.filter((m) => m.id !== tempId));
    }
  };

  const allMessages = [...messages, ...optimisticMessages]
    .filter((message, index, self) => index === self.findIndex((m) => m.id === message.id))
    .sort((a, b) => new Date(a.timeStamp).getTime() - new Date(b.timeStamp).getTime());

  if (isLoading) return <div className="p-4">Loading conversation...</div>;

  return (
    <div className="flex flex-col h-full">
      <div ref={messagesContainerRef} className="flex-1 overflow-y-auto relative" onScroll={handleScroll}>
        {isLoadingMore && (
          <div className="flex justify-center py-2 sticky top-0 z-10">
            <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-gray-400"></div>
          </div>
        )}
        <MessageList messages={allMessages} />
        <div ref={messagesEndRef} />
      </div>
      <MessageInput onSend={handleSendMessage} />
    </div>
  );
}
