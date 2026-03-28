import { IUser } from "../interfaces/IUser.interface";
import { api } from "./ApiClient.service";

interface ILoginResponse {
  user?: any;
  accessToken?: any 
}

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
        const activeUser = {
        userId: response.user.userId || response.user.id,
        name: response.user.name};
      
        localStorage.setItem('activeUser', JSON.stringify(activeUser));
      }

      localStorage.setItem('accessToken', response.accessToken);

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
      localStorage.removeItem('accessToken');
    } catch (error) {
      console.error('Logout error:', error);
    }
  }

}

const authService = new AuthService();
export default authService;