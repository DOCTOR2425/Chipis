import { IMessage } from "./IMessage.interface";

// interfaces/IMessagesResponse.interface.ts
export interface IMessagesResponse {
  messages: IMessage[];
  nextCursor: string | null;
  hasMore: boolean;
}