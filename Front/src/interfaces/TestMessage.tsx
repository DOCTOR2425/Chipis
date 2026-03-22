import { currentUser } from "../services/Auth.service";
import { IChat } from "./IChat.interface";
import { IMessage } from "./IMessage.interface";
import { IUser } from "./IUser.interface";


/*export const currentUser: IUser = {
  id: "1",
  name: "Анна"
};*/
  
export const currentChat: IChat = {
  id: "1",
  name: "Чат с подругой"
};

const user2: IUser = {
  id: "2",
  name: "Мария"
};

// Массив из 4 сообщений
const messages: IMessage[] = [
  {
    id: "1",
    text: "Привет! Как дела?",
    date: "2024-01-15T14:30:00",
    chat: currentChat,
    sender: user2  // Мария пишет Анне
  },
  {
    id: "2",
    text: "Привет! Всё отлично, а у тебя?",
    date: "2024-01-15T14:31:00",
    chat: currentChat,
    sender: currentUser  // Анна отвечает
  },
  {
    id: "3",
    text: "Тоже хорошо! Чем занимаешься?",
    date: "2024-01-15T14:32:00",
    chat: currentChat,
    sender: user2  // Мария
  },
  {
    id: "4",
    text: "Гуляю в парке, погода шикарная ☀️",
    date: "2024-01-15T14:33:00",
    chat: currentChat,
    sender: currentUser  // Анна
  }
];


// Или более короткая версия с встроенными объектами:
const messagesSimple = [
  {
    id: "1",
    text: "Привет! Как дела?",
    date: "14:30",
    chat: { id: "1", name: "Общий чат" },
    sender: { id: "2", name: "Мария" }
  },
  {
    id: "2",
    text: "Привет! Всё отлично, а у тебя?",
    date: "14:31",
    chat: { id: "1", name: "Общий чат" },
    sender: currentUser
  },
  {
    id: "3",
    text: "Тоже хорошо! Чем занимаешься?",
    date: "14:32",
    chat: { id: "1", name: "Общий чат" },
    sender: { id: "2", name: "Мария" }
  },
  {
    id: "4",
    text: "Гуляю в парке, погода шикарная ☀️",
    date: "14:33",
    chat: { id: "1", name: "Общий чат" },
    sender: currentUser
  }
];
export default messagesSimple;
