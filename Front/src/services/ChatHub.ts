import * as signalR from "@microsoft/signalr";
import { IMessage } from "../interfaces/Messages/IMessage.interface";

class ChatHub {
  private connection: signalR.HubConnection | null = null;

  private getToken = (): string => {
    const token = localStorage.getItem("accessToken");
    if (!token) {
      console.warn("Токен не найден в localStorage");
      return "";
    }
    return token;
  }

  async connect(chatId: string | undefined) {
    if (!chatId) {
      console.error("ChatId не указан");
      return false;
    }

    const token = this.getToken();
    if (!token) {
      console.error("Нет токена авторизации");
      return false;
    }

    const BASE_URL = 'https://localhost:7078/api';
    
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${BASE_URL}/chat`, {
        accessTokenFactory: () => {
          const currentToken = localStorage.getItem("accessToken");
          return currentToken || "";
        },
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    // Обработчики событий
    this.connection.on("ReceiveMessage", (message: IMessage) => {
      console.log("Получено сообщение:", message);
    });

    this.connection.onclose((error) => {
      console.log("Соединение закрыто:", error);
    });

    this.connection.onreconnecting((error) => {
      console.log("Переподключение:", error);
    });

    this.connection.onreconnected((connectionId) => {
      console.log("Переподключено:", connectionId);
      // После переподключения снова присоединяемся к чату
      if (chatId) {
        this.connection?.invoke("JoinChat", chatId);
      }
    });

    try {
      await this.connection.start();
      console.log("SignalR подключен");
      
      await this.connection.invoke("JoinChat", chatId);
      console.log("Присоединились к чату:", chatId);
      return true;
    } catch (err) {
      console.error("Ошибка подключения:", err);
      return false;
    }
  }

  async sendMessage(text: string, chatId: string | undefined) {
    if (!this.connection) {
      console.error("Нет соединения");
      return false;
    }
    
    if (this.connection.state !== signalR.HubConnectionState.Connected) {
      console.error("Соединение не активно. Состояние:", this.connection.state);
      return false;
    }
    
    if (!chatId) {
      console.error("ChatId не указан");
      return false;
    }
    
    if (!text || text.trim() === "") {
      console.error("Текст сообщения пуст");
      return false;
    }
    
    try {
      await this.connection.invoke("SendMessage", {
        chatId: chatId,
        text: text.trim()
      });
      console.log("Сообщение отправлено");
      return true;
    } catch (err) {
      console.error("Ошибка отправки:", err);
      return false;
    }
  }

  async disconnect() {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }

  onReceiveMessage(callback: (message: IMessage) => void) {
    if (this.connection) {
      this.connection.off("ReceiveMessage");
      this.connection.on("ReceiveMessage", callback);
    }
  }
}

export const wsManager = new ChatHub();