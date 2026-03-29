import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './pages/Main/App.page';
import Auth from './pages/Auth/Auth.page';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Chat from './components/Chat/Chat';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  // <React.StrictMode>
  <BrowserRouter>
    <title>Fropis</title>
    <Routes>
      <Route path="/" element={<Navigate to="/auth" replace />} />
      <Route path="/chat" element={<App />}>
        <Route path=":chatId" element={<Chat />} /> {/* Конкретный чат */}
      </Route>
      <Route path="/auth" element={<Auth />} />
    </Routes>
  </BrowserRouter>
  // </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();