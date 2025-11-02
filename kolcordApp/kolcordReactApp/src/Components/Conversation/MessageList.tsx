import { MessageDto } from '../../types/types';

export default function MessageList({ messages }: { messages: MessageDto[] }) {
  const currentUserId = localStorage.getItem('userId');
  console.log(messages);

  return (
    <div className="space-y-4 m-6">
      {messages.map((message) => (
        <div key={message.id} className={`flex flex-col ${message.senderId === currentUserId ? 'items-end' : 'items-start'}`}>
          <div className="flex items-center mb-1">
            <span className="font-semibold">{message.senderId === currentUserId ? 'You' : message.senderName}</span>
            <span className="text-xs text-gray-500 ml-2">
              {new Date(message.timeStamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
            </span>
          </div>
          <div className={`rounded-lg p-3 max-w-[80%] ${message.senderId === currentUserId ? 'bg-red-950 text-white' : 'bg-red-900'}`}>
            {message.content}
          </div>
          {message.senderId === currentUserId && <div className="text-xs text-gray-500 mt-1">{message.read ? 'Read' : 'Delivered'}</div>}
        </div>
      ))}
    </div>
  );
}
