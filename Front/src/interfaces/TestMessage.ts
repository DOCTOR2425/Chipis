import { currentUser } from "../services/Auth.service";
import { IChat } from "./IChat.interface";
import { IMessage } from "./Messages/IMessage.interface";
import { IUser } from "./IUser.interface";


export const firstChat: IChat = {
  chatId: "1",
  name: "Чат с подругой"
};

const user2: IUser = {
  userId: "2",
  name: "Мария"
};

export const chatsSimple: IChat[] = [
  firstChat,
  { chatId: "2", name: "Если отобразились" },
  { chatId: "3", name: "Эти чаты" },
  { chatId: "4", name: "То возникла ошибка" },
  { chatId: "5", name: "Мок чат 5" },
  { chatId: "6", name: "Мок чат 6" },
  { chatId: "7", name: "Мок чат 7" },
  { chatId: "8", name: "Мок чат 8" },
  { chatId: "9", name: "Мок чат 9" },
  { chatId: "10", name: "Мок чат 10" }
];
// Массив из 4 сообщений
export const messagesSimple: IMessage[] = [
  {
    messageId: "1",
    text: "Привет! Как дела?",
    sentAt: "2024-01-15T14:30:00",
    chat: firstChat,
    sender: user2  // Мария пишет Анне
  },
  {
    messageId: "2",
    text: "Привет! Всё отлично, а у тебя?",
    sentAt: "2024-01-15T14:31:00",
    chat: firstChat,
    sender: currentUser  // Анна отвечает
  },
  {
    messageId: "3",
    text: "Тоже хорошо! Чем занимаешься?",
    sentAt: "2024-01-15T14:32:00",
    chat: firstChat,
    sender: user2  // Мария
  },
  {
    messageId: "4",
    text: "Гуляю в парке, погода шикарная ☀️",
    sentAt: "2024-01-15T14:33:00",
    chat: firstChat,
    sender: currentUser  // Анна
  }
];
