import React, { FC } from "react";
import { IMessage } from "../../interfaces/Messages/IMessage.interface";
import "./Message.scss";
import { IUser } from "../../interfaces/IUser.interface";
import { notFoundedUser } from "../../contexts/UserContext";

const Message: FC<IMessage> = ({ messageId, text, senderId, sentAt, status = 'delivered', isChanged= false}) => {
  const activeUserStr = localStorage.getItem('activeUser');
  const activeUser: IUser = activeUserStr ? JSON.parse(activeUserStr) : notFoundedUser;

  const getIcon = () => {
    switch (status) {
        case 'sent':
            return <span>🕒</span>;
        case 'delivered':
            return <span>✉️</span>;
        case 'read':
            return <span>🧾</span>;
        default:
            return <span>✉️</span>;
    }
  };

  return (
    <div key={messageId} className={`message message--${senderId === activeUser.userId ? 'user' : 'other'}`}>
      <div className="message__bubble">{text}</div>
      <span className="message__info">
        {isChanged ? <p>изм.</p>: null}
        <div className="message__time">
          {new Date(sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
        </div>
        {getIcon()}
      </span>
    </div>
  );
};

export default Message;