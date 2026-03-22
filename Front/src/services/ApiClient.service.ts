const BASE_URL = 'https://localhost:7078/api';

interface IRequestOptions {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  body?: any;
  headers?: Record<string, string>;
}

interface IRefreshResponse {
  accessToken: string; // сервер возвращает accessToken
}

class ApiClient {
  private baseUrl: string;
  private accessToken: string | null = null;
  private isRefreshing = false;
  private refreshSubscribers: ((token: string) => void)[] = [];

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
    this.loadTokenFromStorage();
  }

  private loadTokenFromStorage(): void {
    const token = localStorage.getItem('accessToken');
    if (token) {
      this.accessToken = token;
    }
  }

  setToken(accessToken: string | null): void {
    this.accessToken = accessToken;
    if (accessToken) {
      localStorage.setItem('accessToken', accessToken);
    } else {
      localStorage.removeItem('accessToken');
    }
  }

  private async refreshAccessToken(): Promise<string> {
    const response = await fetch(`${this.baseUrl}/Auth/refresh`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include', // отправляем cookie с refresh-токеном
    });

    if (!response.ok) {
      throw new Error('Не удалось обновить токен');
    }

    const data: IRefreshResponse = await response.json();
    const newToken = data.accessToken; // поле accessToken, а не token
    this.setToken(newToken);
    return newToken;
  }

  private onRefreshed(newToken: string): void {
    this.refreshSubscribers.forEach(cb => cb(newToken));
    this.refreshSubscribers = [];
  }

  private async handleUnauthorized(
    endpoint: string,
    fetchOptions: RequestInit,
    originalHeaders: Record<string, string>
  ): Promise<Response> {
    if (this.isRefreshing) {
      return new Promise((resolve) => {
        this.refreshSubscribers.push(async (newToken) => {
          const newHeaders = {
            ...originalHeaders,
            Authorization: `Bearer ${newToken}`,
          };
          const retryResponse = await fetch(`${this.baseUrl}${endpoint}`, {
            ...fetchOptions,
            headers: newHeaders,
          });
          resolve(retryResponse);
        });
      });
    }

    this.isRefreshing = true;

    try {
      const newToken = await this.refreshAccessToken();
      this.onRefreshed(newToken);

      const newHeaders = {
        ...originalHeaders,
        Authorization: `Bearer ${newToken}`,
      };
      const retryResponse = await fetch(`${this.baseUrl}${endpoint}`, {
        ...fetchOptions,
        headers: newHeaders,
      });
      return retryResponse;
    } catch (error) {
      this.setToken(null);
      throw new Error('Сессия истекла');
    } finally {
      this.isRefreshing = false;
    }
  }

  private async request<T>(endpoint: string, options: IRequestOptions = {}): Promise<T> {
    const { method = 'GET', body, headers = {} } = options;

    const requestHeaders: Record<string, string> = {
      'Content-Type': 'application/json',
      ...headers,
    };

    if (this.accessToken) {
      requestHeaders['Authorization'] = `Bearer ${this.accessToken}`;
    }

    const fetchOptions: RequestInit = {
      method,
      headers: requestHeaders,
      credentials: 'include',
    };

    if (body) {
      fetchOptions.body = JSON.stringify(body);
    }

    let response: Response;

    try {
      response = await fetch(`${this.baseUrl}${endpoint}`, fetchOptions);
    } catch (networkError: any) {
      if (networkError.message === 'Failed to fetch') {
        throw new Error('Сервер недоступен. Проверьте подключение к интернету');
      }
      throw networkError;
    }

    if (response.status === 401) {
      try {
        response = await this.handleUnauthorized(endpoint, fetchOptions, requestHeaders);
      } catch (refreshError) {
        throw refreshError;
      }
    }

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Ошибка ${response.status}: ${errorText}`);
    }

    return response.json();
  }

  get<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint, { method: 'GET' });
  }

  post<T>(endpoint: string, body: any): Promise<T> {
    return this.request<T>(endpoint, { method: 'POST', body });
  }

  put<T>(endpoint: string, body: any): Promise<T> {
    return this.request<T>(endpoint, { method: 'PUT', body });
  }

  delete<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint, { method: 'DELETE' });
  }
}

export const api = new ApiClient(BASE_URL);