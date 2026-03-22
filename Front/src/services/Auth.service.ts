import { IUser } from "../interfaces/IUser.interface";
import { api } from "./ApiClient.service";


interface ILoginResponse {
  token: string;
  refreshToken: string;
  user?: any;
}

class AuthService{


async loginUser(phone: string, password: string): Promise<ILoginResponse> {
  if (!phone || !password) {
    throw new Error('Телефон и пароль обязательны');
  }

  const cleanPhone = phone.replace(/\s+/g, ' ').trim();

  try {
    // Используем новый api клиент
    const response = await api.post<ILoginResponse>('/Auth/loginUser', {
      Telephone: cleanPhone,
      Password: password,
    });
    // Сохраняем данные в localStorage
    if (response.token) {
      localStorage.setItem('token', response.token);
      api.setToken(response.token, response.refreshToken);
    }
    
    if (response.user) {
      localStorage.setItem('user', JSON.stringify(response.user));
    }

    return response;

  } catch (error: any) {
    if (error.message === 'Failed to fetch') {
      throw new Error('Сервер недоступен. Проверьте подключение к интернету');
    }
    throw new Error(error.message || 'Ошибка при входе');
  }
}

}

const authService = new AuthService();
export default authService;