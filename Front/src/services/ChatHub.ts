import { sendMessage } from "@microsoft/signalr/dist/esm/Utils";
import Message from "../components/Message/Message";
import { IMessage } from "../interfaces/Messages/IMessage.interface";
import * as  signalR from "@microsoft/signalr"

class ChatHub {
  private connection: signalR.HubConnection | null = null;

  async connect(chatId: string | undefined) {
    const BASE_URL = 'https://localhost:7078';

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${BASE_URL}/chat`, {
        accessTokenFactory: () => localStorage.getItem("accessToken")!
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on("ReceiveMessage", (message) => {
      console.log("Получено:", message);
    });

    try {
      await this.connection.start();
      await this.connection.invoke("JoinChat", chatId);
      console.log("Подключено к чату:", chatId);
    } catch (err) {
      console.error("Ошибка подключения:", err);
    }
  }

  async sendMessage(text: string, chatId: string | undefined) {
    if (!this.connection || this.connection.state !== signalR.HubConnectionState.Connected) {
      console.error("Нет подключения к SignalR");
      return;
    }
    
    try {
      await this.connection.invoke("SendMessage", {
        chatId,
        text
      });
    } catch (err) {
      console.error("Ошибка отправки сообщения:", err);
    }
  }
}

export const wsManager = new ChatHub();
