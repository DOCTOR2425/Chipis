import Message from '../Message/Message';
import './Chat.scss';
import { useEffect, useState } from 'react';
import { IMessage } from '../../interfaces/Messages/IMessage.interface';
import { wsManager } from '../../services/SocketManager';
import { useNavigate } from 'react-router-dom';
import avatarImage from '../../media/testImage/avatar1.jpg';
import authService, { currentUser } from '../../services/Auth.service';
import { messagesSimple, firstChat as currentChat } from '../../interfaces/TestMessage';
import { useParams } from 'react-router-dom';
import { chatService } from '../../services/Chat.service';


export default function Chat() {
  const { chatId  } = useParams();

  const [messagesList, setMessagesList] = useState<IMessage[]>([]);
  const [inputText, setInputText] = useState('');

  useEffect(() => {
    const fetchMessages = async () => {
      if (!chatId) return;
      
      try {
        //setLoading(true);
        const data = await chatService.getMessagesFromChat(chatId);  
        setMessagesList(data.messages);
      } catch (err) {
        console.error('Failed to load messages:', err);
        setMessagesList(messagesSimple); // fallback на моки
      } finally {
        //setLoading(false);
      }
    };

    fetchMessages();
    wsManager.connect(currentUser.userId);
  }, [chatId]); // Зависимость от chatId
  
  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
  if (e.key === 'Enter' && !e.shiftKey) {
    e.preventDefault();
    sendMessage();
  }};

  const sendMessage = () => {
    if (inputText.trim() === '') return;
    
    const newMessage: IMessage = {
      messageId: Date.now().toString(),
      text: inputText,
      sentAt: new Date().toISOString(),
      chat: currentChat,
      sender: currentUser
    };
    console.log(newMessage);
    setMessagesList([...messagesList, newMessage]);
    wsManager.sendMessage(newMessage);
    setInputText(''); 
  };

  const handleHeaderClick = () => {
    authService.refreshTest();
  };

    return(
    <div className='Chat'>
            <header className='Chat-header'>
            <div className='Chat-header-info' onClick={handleHeaderClick}>
              <div className='Chat-avatar-wrapper'>
                <img className='User-avatar' src={avatarImage} />
                <div className='Chat-status online'></div>
              </div>
                <div className='Chat-title'>
                    <span>Aboba</span>
                    <span>был(а) только что</span>
                </div>
            </div>
            
            <div className='Chat-header-actions'>
                <button>🔍</button>
                <button>⋮</button>
            </div>
        </header>

        <div className='Chat-body'>
            {messagesList.map(msg => (
              <Message {...msg} key={msg.messageId}></Message>
          )
          )}
        </div>

        <div className='Chat-message-input'>
            <textarea 
            value={inputText} 
            onChange={(e) => setInputText(e.target.value)}
            maxLength={500}
            onKeyDown={handleKeyDown}
            className='Invisible Textarea' 
            placeholder='Напишите что-нибудь...'></textarea>

            <button className='Button' onClick={sendMessage}>Send</button>
        </div>

    </div>
    )
}