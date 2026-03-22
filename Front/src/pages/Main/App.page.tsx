import React from 'react';
import logo from './logo.svg';
import './App.page.scss';
import Chat from '../../components/Chat/Chat';
import AboutMe from '../../components/AboutMe/AboutMe';
import ChatsList from '../../components/ChatsList/ChatsList';
import { Outlet } from 'react-router-dom';

function App() {

   return (
    <div className="App">
      <span className='panel'>
      <AboutMe></AboutMe>
      <ChatsList></ChatsList>
      </span>
  
        <Outlet /> {/* Здесь будет рендериться либо ChatsList, либо Chat */}
      <div></div>
    </div>
  );
}

export default App;
