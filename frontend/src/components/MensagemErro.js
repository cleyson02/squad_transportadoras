import { FiAlertCircle } from 'react-icons/fi';

// Componente simples para exibir mensagens de erro da API.
export default function MensagemErro({ mensagem }) {
  if (!mensagem) return null;
  return (
    <div className="erro">
      <FiAlertCircle /> {mensagem}
    </div>
  );
}

// Le a mensagem de erro vinda da API de forma consistente.
export function extrairErro(error) {
  if (error.response && error.response.data) {
    const data = error.response.data;
    if (typeof data === 'string') return data;
    if (data.mensagem) return data.mensagem;
    if (data.title) return data.title;
  }
  return 'Não foi possível completar a operação. Tente novamente.';
}
