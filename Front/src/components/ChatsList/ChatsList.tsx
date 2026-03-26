import { useEffect, useState } from 'react';
import { IChat } from '../../interfaces/IChat.interface';
import avatarImage from '../../media/testImage/avatar1.jpg';
import './ChatList.scss';
import { chatService } from '../../services/Chat.service';
import { useNavigate } from 'react-router-dom';

export default function ChatsList() {
  const [chats, setChats] = useState<IChat[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();


  useEffect(() => {
    const fetchChats = async () => {
      try {
        setLoading(true);
        const data = await chatService.getChatsOfUser();     
        setChats(data);
        setError(null);
      } 
      catch (err) 
      {
        console.error('Failed to load chats:', err);
        //setError('Не удалось загрузить чаты');
      } finally {
        setLoading(false);
      }
    };

    fetchChats();
  }, []);

  const handleChatClick = (chatId: string, chatName: string) => {
    navigate(`/chat/${chatId}`);
  };

  if (loading) {
    return <div className="chat-list-loading">Загрузка чатов...</div>;
  }

  if (error) {
    return <div className="chat-list-error">{error}</div>;
  }

  if (!chats.length) {
    return <div className="chat-list-empty">У вас пока нет чатов</div>;
  }

  return (
    <ul className="chat-list">
      {chats.map((chat, index) => (
        <li
          key={chat.chatId || index}
          className="chat-item"
          onClick={() => handleChatClick(chat.chatId, chat.name)}
        >
          <img
            src={avatarImage}
            alt={chat.name}
            className="chat-avatar"
          />
          <span className="chat-name">{chat.name}</span>
        </li>
      ))}
    </ul>
  );
}