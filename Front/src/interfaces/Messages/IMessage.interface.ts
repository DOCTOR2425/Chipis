import { IChat } from "../IChat.interface";
import { IUser } from "../IUser.interface";


export interface IMessage {
  messageId: string;
  senderId?: string;
  text: string;
  sentAt: string;
  isChanged: boolean
  status: 'sent' | 'delivered' | 'read'; // возможные статусы
  sender?: IUser;
  chatId?: string;
  chat?: IChat;
}