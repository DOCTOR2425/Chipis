import { IChat } from "../IChat.interface";
import { IUser } from "../IUser.interface";


export interface IMessage {
  messageId: string;
  text: string;
  sentAt: string;
  senderId?: string;
  sender?: IUser;
  chatId?: string;
  chat?: IChat;
}