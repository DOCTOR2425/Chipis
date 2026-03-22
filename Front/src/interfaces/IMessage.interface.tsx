import { IChat } from "./IChat.interface";
import { IUser } from "./IUser.interface";


export interface IMessage {
  id?: string;
  text: string;
  date: Date | string;
  chat: IChat; 
  sender: IUser;          
}