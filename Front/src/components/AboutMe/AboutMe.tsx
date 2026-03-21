import "./AboutMe.scss";
import avatarImage from '../../media/testImage/avatar1.jpg';
import { IUser } from "../../interfaces/IUser.interface";

export const currentUser: IUser = {
  userId: "1",
  name: "Анна"
};

export default function AboutMe() {
  const handleLogout = () => {
    // Логика выхода
    console.log('Выход из аккаунта');
  };

  return (
    <div className="About-me">
        <span>
        <div className="Avatar-wrapper">
          <img className="User-avatar" src={avatarImage} alt={currentUser.name} />
          <div className="User-status" />
        </div>
        <span className="about-me__name">{currentUser.name}</span>
      </span>
      <button className="Button" onClick={handleLogout}>
        <svg className="about-me__logout-icon" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M17 16L21 12M21 12L17 8M21 12L7 12M13 16V17C13 19.2091 11.2091 21 9 21H7C4.79086 21 3 19.2091 3 17V7C3 4.79086 4.79086 3 7 3H9C11.2091 3 13 4.79086 13 7V8" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
        </svg>
        <span>Выйти</span>
      </button>
    </div>
  );
}