import Message from '../Message/Message';
import './Chat.scss';
import messagesSimple, { currentChat, currentUser } from '../../interfaces/TestMessage'
import { useEffect, useState } from 'react';
import { IMessage } from '../../interfaces/IMessage.interface';
import { wsManager } from '../../services/SocketManager';

export default function Chat()
{
const [messagesList, setMessagesList] = useState<IMessage[]>(messagesSimple);
const [inputText, setInputText] = useState('');

  useEffect(() => {
    wsManager.connect(currentUser.userId);

    
    /*fetch('https://localhost:7078/api/ВСТАВЬ-КОНТРОЛЛЕР') //TODO Вставить полный путь после создания контролера
      .then(response => response.json())
      .then(data => setMessagesList(data))
      .catch(error => console.log('Ошибка при загрузке:', error));*/

  }, []);

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
      date: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      chat: currentChat,
      sender: currentUser
    };
    console.log(newMessage)
    setMessagesList([...messagesList, newMessage]);
    wsManager.sendMessage(newMessage);
    setInputText(''); 
  };

    return(
    <div className='Chat'>
        <header className='Chat-header'>
            <div></div>
            <p>Aboba</p>
            <div></div>
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