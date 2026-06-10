import React, { createContext, useContext, useState } from 'react';
import { login as loginRequest } from '../services/authService';

// Context simples para guardar o estado de autenticacao do app.
const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [usuario, setUsuario] = useState(() => localStorage.getItem('usuario'));

  async function entrar(nomeUsuario, senha) {
    const dados = await loginRequest(nomeUsuario, senha);
    localStorage.setItem('token', dados.token);
    localStorage.setItem('usuario', dados.usuario);
    setUsuario(dados.usuario);
  }

  function sair() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    setUsuario(null);
  }

  const autenticado = !!localStorage.getItem('token');

  return (
    <AuthContext.Provider value={{ usuario, autenticado, entrar, sair }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
