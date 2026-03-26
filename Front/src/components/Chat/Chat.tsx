import Message from '../Message/Message';
import './Chat.scss';
import { useEffect, useState } from 'react';
import { IMessage } from '../../interfaces/Messages/IMessage.interface';
import { wsManager } from '../../services/SocketManager';
import avatarImage from '../../media/testImage/avatar1.jpg';
import { useParams } from 'react-router-dom';
import { chatService } from '../../services/Chat.service';
import { IUser } from '../../interfaces/IUser.interface';
import authService from '../../services/Auth.service';
import { notFoundedUser } from '../../contexts/UserContext';

export default function Chat() {
  const { chatId } = useParams();

  const [messagesList, setMessagesList] = useState<IMessage[]>([]);
  const [inputText, setInputText] = useState('');
  const [inputSearchText, setInputSearchText] = useState('');
  const [isSearching, setIsSearching] = useState(false);

  const activeUserStr = localStorage.getItem('activeUser');
  const activeUser: IUser = activeUserStr ? JSON.parse(activeUserStr) : notFoundedUser;

  useEffect(() => {
    const fetchMessages = async () => {
      if (!chatId) return;
      try {
        const data = await chatService.getMessagesFromChat(chatId);
        setMessagesList(data.messages);
      } catch (err) {
        console.error('Failed to load messages:', err);
      }
    };

    fetchMessages();
    wsManager.connect(activeUser.userId);
  }, [chatId, activeUser.userId]);

  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const sendMessage = () => {
    if (inputText.trim() === '') return;

    const newMessage: IMessage = {
      messageId: Date.now().toString(),
      text: inputText,
      sentAt: new Date().toISOString(),
      chatId: chatId || '',
      sender: activeUser,
    };
    setMessagesList([...messagesList, newMessage]);
    wsManager.sendMessage(newMessage);
    setInputText('');
  };

  const handleHeaderClick = () => {
    authService.refreshTest();
  };

  const searchingMessages = () => 
  {
    chatService.searchMessages(inputSearchText);
  }

  return (
    <div className="chat">
      <header className="chat__header">
        <div className="chat__header-info" onClick={handleHeaderClick}>
          <div className="chat__avatar-wrapper">
            <img className="chat__avatar" src={avatarImage} alt="avatar" />
            <div className="chat__status chat__status--online"></div>
          </div>
          <div className="chat__title">
            <span>Aboba</span>
            <span>был(а) только что</span>
          </div>
        </div>

        <div className="chat__header-actions">
          <button className="round-button" onClick={() => setIsSearching(true)}>🔍</button>
          <button className="round-button">⋮</button>
        </div>
      </header>

      <div className={`chat__search-panel ${!isSearching ? 'hidden' : ''} `}>
        <input
          type="text"
          value={inputSearchText}
          onChange={(e) => setInputSearchText(e.target.value)}
          placeholder="Что ищем?.."
        />
        <button className="__btn-close round-button" onClick={searchingMessages}>🔍</button>
        <button className='__btn-close round-button' onClick={() => setIsSearching(false)}>X</button>
      </div>

      <div className="chat__body">
        {messagesList.map((msg) => (
          <Message {...msg} key={msg.messageId} />
        ))}
      </div>

      <div className="chat__message-input">
        <textarea
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          maxLength={500}
          onKeyDown={handleKeyDown}
          className="chat__textarea"
          placeholder="Напишите что-нибудь..."
        />
        <button className="chat__send-button" onClick={sendMessage}>
          Send
        </button>
      </div>
    </div>
  );
}