import { IChat } from "../interfaces/IChat.interface";
import { IMessage } from "../interfaces/Messages/IMessage.interface";
import { IMessagesResponse } from "../interfaces/Messages/IMessagesResponce.interface";
import { api } from "./ApiClient.service";

class ChatService {
  async getChatsOfUser(): Promise<IChat[]> {
    try {
      const response = await api.get<IChat[]>('/Chats/chats');
      return response; 
    } catch (error) {
      console.error('Request ChatsList error:', error);
      throw error;
    }
  }

  async getMessagesFromChat(
    chatId: string, 
    take: number = 50, 
    cursorId?: string
  ): Promise<IMessage[]> {
    try {
      const queryParams: string[] = [];
      queryParams.push(`take=${take}`);
      if (cursorId) {
        queryParams.push(`cursorId=${cursorId}`);
      }
      
      const url = `/Chats/chats/${chatId}/messages${queryParams.length ? `?${queryParams.join('&')}` : ''}`;
      
      const response = await api.get<IMessagesResponse>(url);

      const messages: IMessage[] =  response.messages.map((msg: any) => ({
           ...msg,
          status: msg.isReaded ? 'read' : 'delivered'
        }));

      return messages;
    } catch (error) {
      console.error('Request ChatsList error:', error);
      throw error;
    }
  }


  async searchMessages(text: string): Promise<IMessagesResponse> {
    try {
    const queryParams: string = text;
      const url = `/Chats/chats/search${queryParams}`;
      
      const messages = await api.get<IMessagesResponse>(url);
      return messages;
    } catch (error) {
      console.error('Request ChatsList error:', error);
      throw error;
    }
  }
  
}

export const chatService = new ChatService(); 