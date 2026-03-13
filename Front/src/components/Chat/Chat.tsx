import Message from '../Message/Message';
import './Chat.scss';
import messagesSimple from '../../interfaces/TestMessage'
import { useEffect, useState } from 'react';
import { IMessage } from '../../interfaces/IMessage.interface';

export default function Chat()
{
const [messagesList, setMessagesList] = useState<IMessage[]>([]);

  useEffect(() => {
    fetch('https://localhost:7078/api/ВСТАВЬ-КОНТРОЛЛЕР') //TODO Вставить полный путь после создания контролера
      .then(response => response.json())
      .then(data => setMessagesList(data))
      .catch(error => console.log('Ошибка при загрузке:', error));
  }, []);

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
    </div>
    )
}