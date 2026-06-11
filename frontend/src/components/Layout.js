import { useNavigate } from 'react-router-dom';
import { FiTruck, FiLogOut, FiPlus, FiList } from 'react-icons/fi';
import { useAuth } from '../context/AuthContext';

// Cabecalho e estrutura comum das telas internas.
export default function Layout({ children }) {
  const { usuario, sair } = useAuth();
  const navigate = useNavigate();

  function logout() {
    sair();
    navigate('/login');
  }

  return (
    <div className="layout">
      <header className="topbar">
        <div className="topbar-titulo" onClick={() => navigate('/')}>
          <FiTruck size={22} />
          <span>Gestão de Coletas</span>
        </div>
        <nav className="topbar-nav">
          <button className="btn-link" onClick={() => navigate('/')}>
            <FiList /> Coletas
          </button>
          <button className="btn-link" onClick={() => navigate('/coletas/nova')}>
            <FiPlus /> Nova
          </button>
          <span className="topbar-usuario">{usuario}</span>
          <button className="btn-link" onClick={logout}>
            <FiLogOut /> Sair
          </button>
        </nav>
      </header>
      <main className="conteudo">{children}</main>
    </div>
  );
}
