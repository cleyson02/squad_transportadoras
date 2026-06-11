import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout';
import Login from './pages/Login';
import ListaColetas from './pages/ListaColetas';
import FormColeta from './pages/FormColeta';
import DetalheColeta from './pages/DetalheColeta';
import './App.css';

// Envolve as paginas internas com o Layout e a protecao de rota.
function Privada({ children }) {
  return (
    <ProtectedRoute>
      <Layout>{children}</Layout>
    </ProtectedRoute>
  );
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<Privada><ListaColetas /></Privada>} />
          <Route path="/coletas/nova" element={<Privada><FormColeta /></Privada>} />
          <Route path="/coletas/:id" element={<Privada><DetalheColeta /></Privada>} />
          <Route path="/coletas/:id/editar" element={<Privada><FormColeta /></Privada>} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
