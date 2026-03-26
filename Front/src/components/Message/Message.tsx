import React,{FC} from "react";
import { IMessage } from "../../interfaces/Messages/IMessage.interface"
import "./Message.scss"
import { IUser } from "../../interfaces/IUser.interface";
import { notFoundedUser } from "../../contexts/UserContext";

const Message: FC<IMessage> = ({messageId, text, senderId, sentAt}) =>
{
    const activeUserStr = localStorage.getItem('activeUser');
    const activeUser: IUser = activeUserStr ? JSON.parse(activeUserStr) : notFoundedUser;

    return(
        <div key={messageId} className={`Message Message-${senderId === activeUser.userId ? 'user' : 'other'}`}>   
            <div className="Message-bubble">{text}</div>
            <div className="Message-time">  {new Date(sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
        </div>
    )
};

export default Message;