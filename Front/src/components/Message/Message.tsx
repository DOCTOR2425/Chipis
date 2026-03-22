import React,{FC} from "react";
import { IMessage } from "../../interfaces/Messages/IMessage.interface"
import "./Message.scss"
import { currentUser } from "../../services/Auth.service";

const Message: FC<IMessage> = ({messageId, text, senderId, sentAt}) =>
{
    return(
        <div key={messageId} className={`Message Message-${senderId === currentUser.userId ? 'user' : 'other'}`}>   
            <div className="Message-bubble">{text}</div>
            <div className="Message-time">  {new Date(sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
        </div>
    )
};

export default Message;