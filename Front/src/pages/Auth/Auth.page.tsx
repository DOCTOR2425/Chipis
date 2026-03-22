import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from '../../services/Auth.service';
import './Auth.page.scss';

interface FormData {
  phone: string;
  password: string;
  confirmPassword: string;
  name: string;
}

function Auth() {
  const [isLogin, setIsLogin] = useState<boolean>(true);
  const [formData, setFormData] = useState<FormData>({
    phone: '',
    password: '',
    confirmPassword: '',
    name: ''
  });

  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');

  const navigate = useNavigate();
  const PHONE_MASK = '+375 (__) ___-__-__';

  // Обработчик изменения полей формы
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    // Очищаем ошибку при изменении полей
    setError('');
  };

  // Переключение между формами входа и регистрации
  const toggleAuthMode = () => {
    setIsLogin(!isLogin);
    setError('');
    setFormData({
      phone: '',
      password: '',
      confirmPassword: '',
      name: ''
    });
  };

  // Валидация формы перед отправкой
  const validateForm = (): boolean => {
    if (!formData.phone || !formData.password) {
      setError('Заполните все обязательные поля');
      return false;
    }

    if (!isLogin) {
      if (!formData.name) {
        setError('Введите имя');
        return false;
      }
      if (formData.password !== formData.confirmPassword) {
        setError('Пароли не совпадают');
        return false;
      }
    }

    if (formData.password.length < 6) {
      setError('Пароль должен быть минимум 6 символов');
      return false;
    }

    return true;
  };

  // Отправка формы
 const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
  e.preventDefault();
  
  if (!validateForm()) return;

  setIsLoading(true);
  setError('');

  try {
    const fullPhone = `${formData.phone}`;

    if (isLogin) {
      const response = await authService.loginUser(fullPhone, formData.password);
      console.log('Успешный вход:', response);
    } else {
      console.log('Регистрация:', { ...formData, phone: fullPhone });
      // TODO: добавить метод регистрации в authService
      // await authService.registerUser(fullPhone, formData.password, formData.name);
      alert('Регистрация успешна! Теперь вы можете войти.');
      setIsLogin(true);
    }} 
    catch (err: any) {
      setError(err.message || 'Произошла ошибка');
    }
    finally {
      setIsLoading(false);
      if (isLogin && !error) {
        navigate('/');
      }
    }
  };


  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="auth-header">
          <h2>{isLogin ? 'Добро пожаловать!' : 'Создать аккаунт'}</h2>
          <p>
            {isLogin 
              ? 'Войдите в свой аккаунт' 
              : 'Зарегистрируйтесь для начала работы'
            }
          </p>
        </div>

        <form onSubmit={handleSubmit} className="auth-form">
          {/* Поле имени (только для регистрации) */}
          {!isLogin && (
            <div className="form-group">
              <label htmlFor="name">
                Имя <span className="required">*</span>
              </label>
              <input
                type="text"
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                placeholder="Введите ваше имя"
                disabled={isLoading}
                autoComplete="name"
              />
            </div>
          )}

          {/* Поле телефона */}
          <div className="form-group">
            <label htmlFor="phone">
              Номер телефона <span className="required">*</span>
            </label>
            <input
              type="tel"
              id="phone"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              placeholder={PHONE_MASK}
              disabled={isLoading}
              autoComplete="tel"
              required
            />
          </div>

          {/* Поле пароля */}
          <div className="form-group">
            <label htmlFor="password">
              Пароль <span className="required">*</span>
            </label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="Введите пароль"
              disabled={isLoading}
              autoComplete={isLogin ? "current-password" : "new-password"}
              required
            />
          </div>

          {/* Подтверждение пароля (только для регистрации) */}
          {!isLogin && (
            <div className="form-group">
              <label htmlFor="confirmPassword">
                Подтвердите пароль <span className="required">*</span>
              </label>
              <input
                type="password"
                id="confirmPassword"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                placeholder="Повторите пароль"
                disabled={isLoading}
                autoComplete="new-password"
                required
              />
            </div>
          )}

          {/* Ссылка "Забыли пароль" (только для входа) */}
          {isLogin && (
            <div className="forgot-password">
              <a href="/forgot-password">Забыли пароль?</a>
            </div>
          )}

          {/* Кнопка отправки */}
          <div>
          
        {error && (
          <div className="error-message">
            {error}
          </div>
        )}


          <button 
            type="submit" 
            className="auth-button"
            disabled={isLoading}
          >
            {isLoading 
              ? 'Загрузка...' 
              : (isLogin ? 'Войти' : 'Зарегистрироваться')
            }
          </button>
          </div>
        </form>

        {/* Переключение между формами */}
        <div className="auth-footer">
          <p>
            {isLogin ? 'Нет аккаунта?' : 'Уже есть аккаунт?'}
            <button 
              className="toggle-button"
              onClick={toggleAuthMode}
              type="button"
              disabled={isLoading}
            >
              {isLogin ? 'Создать аккаунт' : 'Войти'}
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Auth;