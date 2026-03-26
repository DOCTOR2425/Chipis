import { createContext, useContext, useState, useEffect } from 'react';
import { IUser } from '../interfaces/IUser.interface';

export const notFoundedUser: IUser ={
  userId: "-1",
  name: "Не найден"
};

interface UserContextType {
  user: IUser;
  setUser: (user: IUser) => void;
}

// Создаем контекст вне компонента
const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<IUser>(notFoundedUser);

  useEffect(() => {
    const activeUserStr = localStorage.getItem('activeUser');
    if (activeUserStr) {
      setUser(JSON.parse(activeUserStr));
    }
  }, []);
  
  /*return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );*/
};

export const useUser = () => {
  const context = useContext(UserContext);
  if (context === undefined) {
    return { user: notFoundedUser, setUser: () => {} };
  }
  return context;
};