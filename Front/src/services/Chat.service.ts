import { IChat } from "../interfaces/IChat.interface";
import { IMessage } from "../interfaces/Messages/IMessage.interface";
import { IMessagesResponse } from "../interfaces/Messages/IMessagesResponce.interface";
import { api } from "./ApiClient.service";

class ChatService {
  async getChatsOfUser(): Promise<IChat[]> {
    try {
      const response = await api.get<IChat[]>('/Chats/chast');
      return response; 
    } catch (error) {
      console.error('Request ChatsList error:', error);
      throw error;
    }
  }

  async getMessagesFromChat(chatId: string): Promise<IMessagesResponse> {
    try {
      const response = await api.get<IMessagesResponse>(`/Chats/chats/${chatId}/messages`);
      return response; 
    } catch (error) {
      console.error('Request ChatsList error:', error);
      throw error;
    }
  }
  
}

export const chatService = new ChatService(); 