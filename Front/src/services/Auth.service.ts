import { IUser } from "../interfaces/IUser.interface";
import { api } from "./ApiClient.service";

interface ILoginResponse {
  user?: any;
  accessToken?: any 
}

export var currentUser: IUser = {
  id: "1",
  name: "Анна"
};

class AuthService {
  
  async loginUser(phone: string, password: string): Promise<ILoginResponse> {
    if (!phone || !password) {
      throw new Error('Телефон и пароль обязательны');
    }

    const cleanPhone = phone.replace(/\s+/g, ' ').trim();

    try {
      const response = await api.post<ILoginResponse>('/Auth/loginUser', {
        Telephone: cleanPhone,
        Password: password,
      });

      if(response.user)
      {
        currentUser = response.user;
      }

      return response;
    } catch (error: any) {
      console.log("AuthService error:", error.message);
      if (error.message === 'Failed to fetch') {
        throw new Error('Сервер недоступен. Проверьте подключение к интернету');
      }
      throw new Error(error.message || 'Ошибка при входе');
    }
  }


  // Метод для выхода (удаляем куку через запрос на сервер)
  async logout(): Promise<void> {
    try {
      await api.post('/Auth/logout', {});
      // очищаем localStorage от данных пользователя
      localStorage.removeItem('accessToken');
      // Если нужно, перенаправляем на страницу входа
    } catch (error) {
      console.error('Logout error:', error);
    }
  }


    async refreshTest(): Promise<void> {
    console.log(await api.get('/Users/testAuth'));
  }
}

const authService = new AuthService();
export default authService;