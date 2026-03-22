import React,{FC} from "react";
import { IMessage } from "../../interfaces/IMessage.interface"
import "./Message.scss"
import { currentUser } from "../../services/Auth.service";

const Message: FC<IMessage> = ({id, text, sender, date}) =>
{
    return(
        <div key={id} className={`Message Message-${sender.id === currentUser.id ? 'user' : 'other'}`}>   
            <div className="Message-bubble">{text}</div>
            <div className="Message-time">{date.toString()}</div>
        </div>
    )
};

export default Message;