import { IMessage } from "../interfaces/Messages/IMessage.interface";

class SocketManager {
    
    private ws: WebSocket | null = null;
    private id: string = '';

    connect(id: string) {
    this.id = id;

    // URL C# бэкенда
    const wsUrl = `wss://localhost:7078/ws?id=${id}`;
    
    this.ws = new WebSocket(wsUrl);

    this.ws.onopen = () => {
      console.log('Подключено к серверу');
    };

    this.ws.onclose = () => {
      console.log('Соединение закрыто');
    };

    this.ws.onerror = (error) => {
      console.error('Ошибка WebSocket:', error);
    };

    this.ws.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log('Получено:', data);
    };
  }

  sendMessage(message: IMessage) 
  {
    if (!this.ws || this.ws.readyState !== WebSocket.OPEN) {

      console.error('Нет соединения');
      return false;
    }

    const request = {
        ChatId: message.chatId,
        SenderId: (message.sender) ? message.sender.userId : message.senderId,
        Text: message.text
    };

    this.ws.send(JSON.stringify(request));
    console.log(' Отправлено:', message);
    return true;
  }


  disconnect() 
  {
    this.ws?.close();
    this.ws = null;
  }

}

//export const wsManager = new SocketManager();