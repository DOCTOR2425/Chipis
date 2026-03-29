import * as  signalR from "@microsoft/signalr"

class ChatHub {
  private connection: signalR.HubConnection | null = null;
  private BASE_URL = 'https://localhost:7078';

  async connect(chatId: string | undefined) {
    if (this.connection && this.connection.state !== signalR.HubConnectionState.Disconnected) {
      console.warn("SignalR уже подключён, пропускаю connect()");
      return;
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.BASE_URL}/chat`, {
        accessTokenFactory: () => localStorage.getItem("accessToken") || ""
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on("ReceiveMessage", (message) => {
      console.log("Получено:", message);
    });

    this.connection.onreconnected(async () => {
      console.log("Соединение установлено или восстановлено");

      if (chatId) {
        await this.connection!.invoke("JoinChat", chatId);
        console.log("Подключено к чату:", chatId);
      }
    });

    try {
      await this.connection.start();
      console.log("SignalR стартовал, ждём onreconnected...");
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
