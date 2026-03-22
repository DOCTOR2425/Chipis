import Message from '../Message/Message';
import './Chat.scss';
import messagesSimple, { currentChat } from '../../interfaces/TestMessage'
import { useEffect, useState } from 'react';
import { IMessage } from '../../interfaces/IMessage.interface';
import { wsManager } from '../../services/SocketManager';
import { useNavigate } from 'react-router-dom';
import avatarImage from '../../media/testImage/avatar1.jpg';
import authService, { currentUser } from '../../services/Auth.service';
export default function Chat()
{

const [messagesList, setMessagesList] = useState<IMessage[]>(messagesSimple);
const [inputText, setInputText] = useState('');

  /*useEffect(() => {
    wsManager.connect(currentUser.id);
  }, []);*/
  
  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
  if (e.key === 'Enter' && !e.shiftKey) {
    e.preventDefault();
    sendMessage();
  }};

  const sendMessage = () => {
    if (inputText.trim() === '') return;
    
    const newMessage: IMessage = {
      id: Date.now().toString(),
      text: inputText,
      date: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      chat: currentChat,
      sender: currentUser
    };
    console.log(newMessage)
    setMessagesList([...messagesList, newMessage]);
    wsManager.sendMessage(newMessage);
    setInputText(''); 
  };

  const navigate = useNavigate();

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
              <Message {...msg} key={msg.id}></Message>
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