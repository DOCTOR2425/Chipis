import { IMessage } from "../interfaces/IMessage.interface";

class SocketManager {
    
    private ws: WebSocket | null = null;
    private userId: string = '';

    connect(userId: string) {
    this.userId = userId;

    // URL C# бэкенда
    const wsUrl = `ws://localhost:7078/ws?`;//userId=${userId}`;
    
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

  sendMessage(message: IMessage) {
  if (!this.ws || this.ws.readyState !== WebSocket.OPEN) {

    console.error('Нет соединения');
    return false;
  }

  // Отправляем сообщение прямо как есть
  this.ws.send(JSON.stringify(message));
  console.log(' Отправлено:', message);
  return true;
    }

   disconnect() {
    this.ws?.close();
    this.ws = null;
  }

}

export const wsManager = new SocketManager();