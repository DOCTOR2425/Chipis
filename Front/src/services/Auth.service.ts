import { IUser } from "../interfaces/IUser.interface";

interface ILoginResponse {
  token: string;
  user: IUser;
  message?: string;
}

class AuthService{

  private baseUrl: string;

  constructor() {
    this.baseUrl = 'http://localhost:5000/api';
  }


 async loginUser(phone: string, password: string): Promise<ILoginResponse> {

  if (!phone || !password) {
    throw new Error('Телефон и пароль обязательны');
  }

  // Очищаем телефон от лишних пробелов
  const cleanPhone = phone.replace(/\s+/g, ' ').trim();

  try {
    const response = await fetch(`${this.baseUrl}/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        phone: cleanPhone,
        password: password,
      }),
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || 'Ошибка при входе');
    }

    // Сохраняем данные в localStorage
    if (data.token) {
      localStorage.setItem('token', data.token);
    }
    
    if (data.user) {
      localStorage.setItem('user', JSON.stringify(data.user));
    }


    return data;

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