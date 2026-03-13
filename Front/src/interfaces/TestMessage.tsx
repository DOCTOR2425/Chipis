import { IChat } from "./IChat.interface";
import { IMessage } from "./IMessage.interface";
import { IUser } from "./IUser.interface";


export const currentUser: IUser = {
  userId: "1",
  name: "Анна"
};
  
export const currentChat: IChat = {
  chatId: "1",
  name: "Чат с подругой"
};

const user2: IUser = {
  userId: "2",
  name: "Мария"
};

// Массив из 4 сообщений
const messages: IMessage[] = [
  {
    messageId: "1",
    text: "Привет! Как дела?",
    date: "2024-01-15T14:30:00",
    chat: currentChat,
    sender: user2  // Мария пишет Анне
  },
  {
    messageId: "2",
    text: "Привет! Всё отлично, а у тебя?",
    date: "2024-01-15T14:31:00",
    chat: currentChat,
    sender: currentUser  // Анна отвечает
  },
  {
    messageId: "3",
    text: "Тоже хорошо! Чем занимаешься?",
    date: "2024-01-15T14:32:00",
    chat: currentChat,
    sender: user2  // Мария
  },
  {
    messageId: "4",
    text: "Гуляю в парке, погода шикарная ☀️",
    date: "2024-01-15T14:33:00",
    chat: currentChat,
    sender: currentUser  // Анна
  }
];


// Или более короткая версия с встроенными объектами:
const messagesSimple = [
  {
    messageId: "1",
    text: "Привет! Как дела?",
    date: "14:30",
    chat: { chatId: "1", name: "Общий чат" },
    sender: { userId: "2", name: "Мария" }
  },
  {
    messageId: "2",
    text: "Привет! Всё отлично, а у тебя?",
    date: "14:31",
    chat: { chatId: "1", name: "Общий чат" },
    sender: currentUser
  },
  {
    messageId: "3",
    text: "Тоже хорошо! Чем занимаешься?",
    date: "14:32",
    chat: { chatId: "1", name: "Общий чат" },
    sender: { userId: "2", name: "Мария" }
  },
  {
    messageId: "4",
    text: "Гуляю в парке, погода шикарная ☀️",
    date: "14:33",
    chat: { chatId: "1", name: "Общий чат" },
    sender: currentUser
  }
];
export default messagesSimple;
