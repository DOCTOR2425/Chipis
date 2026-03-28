import Message from "../components/Message/Message";
import { IMessage } from "../interfaces/Messages/IMessage.interface";
import * as  signalR from "@microsoft/signalr"

class ChatHub{
    
  
    async connect() 
    {

      const BASE_URL = 'https://localhost:7078/api';

      var connection = new signalR.HubConnectionBuilder()
      .withUrl(`${BASE_URL}/chat`)
      .withAutomaticReconnect()
      .build()
    
      connection.on("RecieveMessage", (message) => {
        console.log(message)
      })
      try{

        await connection.start();
        await connection.invoke("OpenChat");

      }
      catch(err)
      {
        console.log("Ошибка подключения" + err);
      }
    };
  }


export const wsManager = new ChatHub();