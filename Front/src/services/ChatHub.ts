import * as  signalR from "@microsoft/signalr"

class ChatHub {
  private connection: signalR.HubConnection | null = null;
  private BASE_URL = 'https://localhost:7078';


  async connect(chatId: string | undefined) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.BASE_URL}/chat`, {
        accessTokenFactory: () => localStorage.getItem("accessToken") || ""
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on("ReceiveMessage", (message => {
      console.log(message);
    }));

    this.connection.on("OnIncomingChat", (message => {
      console.log(message);
    }));

    try {
      await this.connection.start();
      await this.connection.invoke("JoinChat", chatId);
      console.log(this.connection);
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

  async disconnect() {
    if (this.connection) {
      console.log("Отключаю SignalR");
      try {
        await this.connection.stop();
      } catch (e) {
        console.warn("Ошибка при остановке соединения:", e);
      }
      this.connection = null;
    }
  }
}

export const wsManager = new ChatHub();
