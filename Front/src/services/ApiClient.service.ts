// api-client.ts

const BASE_URL = 'https://localhost:7078/api';

interface IRequestOptions {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  body?: any;
  headers?: Record<string, string>;
}

interface IRefreshResponse {
  token: string;
  refreshToken: string;
}

class ApiClient {
  private baseUrl: string;
  private token: string | null = null;
  private refreshToken: string | null = null;
  private isRefreshing = false;
  private refreshSubscribers: ((token: string) => void)[] = [];

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  setToken(token: string | null, refreshToken?: string | null): void {
    this.token = token;
    if (refreshToken !== undefined) {
      this.refreshToken = refreshToken;
    }
  }

  private async refreshAccessToken(): Promise<string> {
    const response = await fetch(`${this.baseUrl}/Auth/refresh`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken: this.refreshToken }),
    });

    if (!response.ok) {
      throw new Error('Refresh failed');
    }

    const data: IRefreshResponse = await response.json();
    this.token = data.token;
    this.refreshToken = data.refreshToken;
    return data.token;
  }

  private onRefreshed(token: string): void {
    this.refreshSubscribers.forEach(cb => cb(token));
    this.refreshSubscribers = [];
  }

  private async request<T>(endpoint: string, options: IRequestOptions = {}): Promise<T> {
    const { method = 'GET', body, headers = {} } = options;

    const requestHeaders: Record<string, string> = {
      'Content-Type': 'application/json',
      ...headers,
    };

    if (this.token) {
      requestHeaders['Authorization'] = `Bearer ${this.token}`;
    }

    console.log('1');
    const fetchOptions: RequestInit = {
      method,
      headers: requestHeaders,
    };

    if (body) {
      fetchOptions.body = JSON.stringify(body);
    }

    let response: Response;
    
    response = await fetch(`${this.baseUrl}${endpoint}`, fetchOptions);

    if (response.status === 401 && this.refreshToken) {
      if (this.isRefreshing) {
        return new Promise((resolve) => {
          this.refreshSubscribers.push((newToken) => {
            fetchOptions.headers = {
              ...fetchOptions.headers,
              Authorization: `Bearer ${newToken}`,
            };
            resolve(fetch(`${this.baseUrl}${endpoint}`, fetchOptions).then(res => res.json()));
          });
        });
      }

      this.isRefreshing = true;

      try {
        const newToken = await this.refreshAccessToken();
        this.onRefreshed(newToken);
        fetchOptions.headers = {
          ...fetchOptions.headers,
          Authorization: `Bearer ${newToken}`,
        };
        response = await fetch(`${this.baseUrl}${endpoint}`, fetchOptions);
      } 
      catch (error) {
        this.setToken(null, null);
        throw new Error('Session expired');
      } finally {
        this.isRefreshing = false;
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