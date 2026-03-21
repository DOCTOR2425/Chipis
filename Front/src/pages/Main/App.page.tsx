import React from 'react';
import logo from './logo.svg';
import './App.page.scss';
import Chat from '../../components/Chat/Chat';
import AboutMe from '../../components/AboutMe/AboutMe';

function App() {
  return (
    
    <div className='App'>
      <AboutMe></AboutMe>
      <Chat></Chat>
      <div></div>
    </div>
  );
}

export default App;
