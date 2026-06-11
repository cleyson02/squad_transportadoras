import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { FiSave, FiArrowLeft } from 'react-icons/fi';
import { criarColeta, atualizarColeta, obterColeta, listarClientes } from '../services/coletasService';
import MensagemErro, { extrairErro } from '../components/MensagemErro';

export default function FormColeta() {
  const navigate = useNavigate();
  const { id } = useParams();
  const edicao = !!id;

  const [remetente, setRemetente] = useState('');
  const [destinatario, setDestinatario] = useState('');
  const [dataRetirada, setDataRetirada] = useState('');
  const [prioridade, setPrioridade] = useState(2);
  const [observacao, setObservacao] = useState('');
  const [clienteId, setClienteId] = useState('');
  const [clientes, setClientes] = useState([]);
  const [erro, setErro] = useState('');
  const [salvando, setSalvando] = useState(false);

  useEffect(() => {
    listarClientes().then((r) => setClientes(r.data)).catch(() => {});
  }, []);

  useEffect(() => {
    if (edicao) {
      obterColeta(id)
        .then((r) => {
          const c = r.data;
          setRemetente(c.remetente);
          setDestinatario(c.destinatario);
          // datetime-local espera o formato yyyy-MM-ddTHH:mm
          setDataRetirada(c.dataRetirada ? c.dataRetirada.substring(0, 16) : '');
          setPrioridade(c.prioridade);
          setObservacao(c.observacao || '');
          setClienteId(c.clienteId || '');
        })
        .catch((error) => setErro(extrairErro(error)));
    }
  }, [edicao, id]);

  async function handleSubmit(e) {
    e.preventDefault();
    setErro('');
    setSalvando(true);
    try {
      const dados = {
        remetente,
        destinatario,
        dataRetirada,
        prioridade: Number(prioridade),
        observacao: observacao || null,
        clienteId: clienteId ? Number(clienteId) : null,
      };
      if (edicao) {
        await atualizarColeta(id, dados);
        navigate(`/coletas/${id}`);
      } else {
        await criarColeta(dados);
        navigate('/');
      }
    } catch (error) {
      setErro(extrairErro(error));
    } finally {
      setSalvando(false);
    }
  }

  return (
    <div>
      <div className="pagina-titulo">
        <h2>{edicao ? 'Editar coleta' : 'Nova coleta'}</h2>
        <button className="btn-link" onClick={() => navigate(-1)}>
          <FiArrowLeft /> Voltar
        </button>
      </div>

      <MensagemErro mensagem={erro} />

      <form className="formulario" onSubmit={handleSubmit}>
        <div className="campo">
          <label>Remetente *</label>
          <input value={remetente} onChange={(e) => setRemetente(e.target.value)} required maxLength={100} />
        </div>

        <div className="campo">
          <label>Destinatário *</label>
          <input value={destinatario} onChange={(e) => setDestinatario(e.target.value)} required maxLength={100} />
        </div>

        <div className="campo">
          <label>Cliente</label>
          <select value={clienteId} onChange={(e) => setClienteId(e.target.value)}>
            <option value="">Selecione</option>
            {clientes.map((c) => (
              <option key={c.id} value={c.id}>{c.nome}</option>
            ))}
          </select>
        </div>

        <div className="campo">
          <label>Data/hora prevista *</label>
          <input type="datetime-local" value={dataRetirada} onChange={(e) => setDataRetirada(e.target.value)} required />
        </div>

        <div className="campo">
          <label>Prioridade *</label>
          <select value={prioridade} onChange={(e) => setPrioridade(e.target.value)}>
            <option value={1}>Baixa</option>
            <option value={2}>Normal</option>
            <option value={3}>Alta</option>
          </select>
        </div>

        <div className="campo">
          <label>Observação</label>
          <textarea value={observacao} onChange={(e) => setObservacao(e.target.value)} maxLength={500} rows={3} />
        </div>

        <button type="submit" className="btn btn-primario" disabled={salvando}>
          <FiSave /> {salvando ? 'Salvando...' : 'Salvar'}
        </button>
      </form>
    </div>
  );
}
