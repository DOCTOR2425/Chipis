import React,{FC} from "react";
import { IMessage } from "../../interfaces/IMessage.interface"
import "./Message.scss"

const Message: FC<IMessage> = ({messageId, text, sender, date}) =>
{
    return(
        <div key={messageId} className={`Message Message-${sender.userId === '1' ? 'user' : 'other'}`}>   
            <div className="Message-bubble">{text}</div>
            <div className="Message-time">{date.toString()}</div>
        </div>
    )
};

export default Message;