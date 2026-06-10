import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { FiSearch, FiEye, FiPlus, FiAlertTriangle, FiChevronLeft, FiChevronRight } from 'react-icons/fi';
import { listarColetas, listarClientes } from '../services/coletasService';
import { nomeStatus, nomePrioridade, classeStatus, ehPrioridadeAlta, formatarData, STATUS } from '../services/labels';
import MensagemErro, { extrairErro } from '../components/MensagemErro';

export default function ListaColetas() {
  const navigate = useNavigate();
  const [coletas, setColetas] = useState([]);
  const [clientes, setClientes] = useState([]);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);

  // Filtros e paginacao.
  const [status, setStatus] = useState('');
  const [clienteId, setClienteId] = useState('');
  const [dataInicio, setDataInicio] = useState('');
  const [dataFim, setDataFim] = useState('');
  const [pagina, setPagina] = useState(1);
  const [totalPaginas, setTotalPaginas] = useState(1);

  const buscar = useCallback(async () => {
    setErro('');
    setCarregando(true);
    try {
      const filtros = {
        pagina,
        tamanhoPagina: 10,
      };
      if (status) filtros.status = status;
      if (clienteId) filtros.clienteId = clienteId;
      if (dataInicio) filtros.dataInicio = dataInicio;
      if (dataFim) filtros.dataFim = dataFim;

      const resposta = await listarColetas(filtros);
      setColetas(resposta.data.itens);
      setTotalPaginas(resposta.data.totalPaginas || 1);
    } catch (error) {
      setErro(extrairErro(error));
    } finally {
      setCarregando(false);
    }
  }, [pagina, status, clienteId, dataInicio, dataFim]);

  useEffect(() => {
    buscar();
  }, [buscar]);

  useEffect(() => {
    listarClientes().then((r) => setClientes(r.data)).catch(() => {});
  }, []);

  function aplicarFiltros(e) {
    e.preventDefault();
    setPagina(1);
    buscar();
  }

  return (
    <div>
      <div className="pagina-titulo">
        <h2>Coletas</h2>
        <button className="btn btn-primario" onClick={() => navigate('/coletas/nova')}>
          <FiPlus /> Nova coleta
        </button>
      </div>

      <form className="filtros" onSubmit={aplicarFiltros}>
        <div className="campo">
          <label>Situação</label>
          <select value={status} onChange={(e) => setStatus(e.target.value)}>
            <option value="">Todas</option>
            {Object.entries(STATUS).map(([valor, texto]) => (
              <option key={valor} value={valor}>{texto}</option>
            ))}
          </select>
        </div>

        <div className="campo">
          <label>Cliente</label>
          <select value={clienteId} onChange={(e) => setClienteId(e.target.value)}>
            <option value="">Todos</option>
            {clientes.map((c) => (
              <option key={c.id} value={c.id}>{c.nome}</option>
            ))}
          </select>
        </div>

        <div className="campo">
          <label>De</label>
          <input type="date" value={dataInicio} onChange={(e) => setDataInicio(e.target.value)} />
        </div>

        <div className="campo">
          <label>Até</label>
          <input type="date" value={dataFim} onChange={(e) => setDataFim(e.target.value)} />
        </div>

        <button type="submit" className="btn">
          <FiSearch /> Filtrar
        </button>
      </form>

      <MensagemErro mensagem={erro} />

      <table className="tabela">
        <thead>
          <tr>
            <th>Número</th>
            <th>Remetente</th>
            <th>Status</th>
            <th>Prioridade</th>
            <th>Previsão</th>
            <th>Motorista</th>
            <th>Veículo</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {coletas.map((c) => (
            <tr key={c.id} className={ehPrioridadeAlta(c.prioridade) ? 'linha-alta' : ''}>
              <td>{c.numeroSolicitacao}</td>
              <td>{c.remetente}</td>
              <td><span className={classeStatus(c.status)}>{nomeStatus(c.status)}</span></td>
              <td>
                {ehPrioridadeAlta(c.prioridade) && <FiAlertTriangle className="icone-alta" title="Prioridade alta" />}
                {nomePrioridade(c.prioridade)}
              </td>
              <td>{formatarData(c.dataRetirada)}</td>
              <td>{c.motorista ? c.motorista.nome : '-'}</td>
              <td>{c.veiculo ? c.veiculo.placa : '-'}</td>
              <td>
                <button className="btn-link" onClick={() => navigate(`/coletas/${c.id}`)}>
                  <FiEye /> Detalhes
                </button>
              </td>
            </tr>
          ))}
          {!carregando && coletas.length === 0 && (
            <tr><td colSpan="8" className="vazio">Nenhuma coleta encontrada.</td></tr>
          )}
        </tbody>
      </table>

      <div className="paginacao">
        <button className="btn" disabled={pagina <= 1} onClick={() => setPagina(pagina - 1)}>
          <FiChevronLeft /> Anterior
        </button>
        <span>Página {pagina} de {totalPaginas}</span>
        <button className="btn" disabled={pagina >= totalPaginas} onClick={() => setPagina(pagina + 1)}>
          Próxima <FiChevronRight />
        </button>
      </div>
    </div>
  );
}
