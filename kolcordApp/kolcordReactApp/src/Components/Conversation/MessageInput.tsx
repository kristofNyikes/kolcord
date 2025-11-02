import { useState, useRef, KeyboardEvent } from 'react';

export default function MessageInput({ onSend }: { onSend: (content: string) => void }) {
  const [message, setMessage] = useState('');
  const textareaRef = useRef<HTMLTextAreaElement>(null);
  const MAX_CHARS = 1500;

  const handleKeyDown = (e: KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSubmit(e as unknown as React.FormEvent);
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (message.trim() && message.length <= MAX_CHARS) {
      onSend(message);
      setMessage('');
      if (textareaRef.current) {
        textareaRef.current.style.height = 'auto';
      }
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    if (e.target.value.length <= MAX_CHARS) {
      setMessage(e.target.value);
      e.target.style.height = 'auto';
      e.target.style.height = `${Math.min(e.target.scrollHeight, 96)}px`;
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4 border-t border-gray-700">
      <div className="flex flex-col">
        <div className="flex items-end">
          <textarea
            ref={textareaRef}
            value={message}
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            className={`flex-1 border-none rounded-l-lg p-2 bg-stone-950 resize-none overflow-y-auto max-h-24 ${
              message.length >= MAX_CHARS * 0.9 ? 'ring-1 ring-yellow-500' : ''
            } ${message.length === MAX_CHARS ? 'ring-1 ring-red-500' : ''}`}
            placeholder="Type a message..."
            style={{ minHeight: '44px' }}
            rows={1}
          />
          <button
            type="submit"
            className="bg-red-950 text-white px-4 mx-3 rounded h-11 disabled:opacity-50"
            disabled={!message.trim() || message.length > MAX_CHARS}
          >
            Send
          </button>
        </div>
        <div
          className={`text-xs mt-1 text-right ${
            message.length >= MAX_CHARS ? 'text-red-500' : message.length >= MAX_CHARS * 0.9 ? 'text-yellow-500' : 'text-gray-400'
          }`}
        >
          {message.length}/{MAX_CHARS}
        </div>
      </div>
    </form>
  );
}
