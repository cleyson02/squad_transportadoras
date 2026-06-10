import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FiTruck, FiLogIn } from 'react-icons/fi';
import { useAuth } from '../context/AuthContext';
import MensagemErro, { extrairErro } from '../components/MensagemErro';

export default function Login() {
  const [usuario, setUsuario] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);
  const { entrar } = useAuth();
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setErro('');
    setCarregando(true);
    try {
      await entrar(usuario, senha);
      navigate('/');
    } catch (error) {
      setErro(extrairErro(error));
    } finally {
      setCarregando(false);
    }
  }

  return (
    <div className="login-tela">
      <form className="login-card" onSubmit={handleSubmit}>
        <div className="login-titulo">
          <FiTruck size={28} />
          <h1>Gestão de Coletas</h1>
        </div>

        <label>Usuário</label>
        <input
          type="text"
          value={usuario}
          onChange={(e) => setUsuario(e.target.value)}
          placeholder="admin"
          required
        />

        <label>Senha</label>
        <input
          type="password"
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          placeholder="Admin@123"
          required
        />

        <MensagemErro mensagem={erro} />

        <button type="submit" className="btn btn-primario" disabled={carregando}>
          <FiLogIn /> {carregando ? 'Entrando...' : 'Entrar'}
        </button>
      </form>
    </div>
  );
}
