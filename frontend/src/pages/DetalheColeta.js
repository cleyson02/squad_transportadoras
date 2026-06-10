import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  FiArrowLeft, FiEdit, FiUserPlus, FiPlayCircle, FiCheckCircle,
  FiXCircle, FiPlusCircle, FiAlertTriangle,
} from 'react-icons/fi';
import {
  obterColeta, listarMotoristas, listarVeiculos,
  atribuirMotoristaVeiculo, iniciarColeta, concluirColeta,
  cancelarColeta, registrarOcorrencia,
} from '../services/coletasService';
import {
  nomeStatus, nomePrioridade, classeStatus, ehPrioridadeAlta, formatarData,
} from '../services/labels';
import MensagemErro, { extrairErro } from '../components/MensagemErro';

export default function DetalheColeta() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [coleta, setColeta] = useState(null);
  const [motoristas, setMotoristas] = useState([]);
  const [veiculos, setVeiculos] = useState([]);
  const [erro, setErro] = useState('');

  const [motoristaId, setMotoristaId] = useState('');
  const [veiculoId, setVeiculoId] = useState('');
  const [descricaoOcorrencia, setDescricaoOcorrencia] = useState('');

  const carregar = useCallback(async () => {
    setErro('');
    try {
      const r = await obterColeta(id);
      setColeta(r.data);
      setMotoristaId(r.data.motoristaId || '');
      setVeiculoId(r.data.veiculoId || '');
    } catch (error) {
      setErro(extrairErro(error));
    }
  }, [id]);

  useEffect(() => {
    carregar();
    listarMotoristas().then((r) => setMotoristas(r.data)).catch(() => {});
    listarVeiculos().then((r) => setVeiculos(r.data)).catch(() => {});
  }, [carregar]);

  async function executar(acao) {
    setErro('');
    try {
      await acao();
      await carregar();
    } catch (error) {
      setErro(extrairErro(error));
    }
  }

  if (!coleta) {
    return (
      <div>
        <MensagemErro mensagem={erro} />
        {!erro && <p>Carregando...</p>}
      </div>
    );
  }

  const cancelada = coleta.status === 5;
  const coletada = coleta.status === 4;
  const temMotoristaVeiculo = coleta.motoristaId && coleta.veiculoId;

  return (
    <div>
      <div className="pagina-titulo">
        <h2>Coleta {coleta.numeroSolicitacao}</h2>
        <button className="btn-link" onClick={() => navigate('/')}>
          <FiArrowLeft /> Voltar
        </button>
      </div>

      <MensagemErro mensagem={erro} />

      <div className="card">
        <h3>Dados da coleta</h3>
        <div className="dados-grid">
          <div><strong>Remetente:</strong> {coleta.remetente}</div>
          <div><strong>Destinatário:</strong> {coleta.destinatario}</div>
          <div><strong>Cliente:</strong> {coleta.cliente ? coleta.cliente.nome : '-'}</div>
          <div><strong>Previsão:</strong> {formatarData(coleta.dataRetirada)}</div>
          <div>
            <strong>Status:</strong>{' '}
            <span className={classeStatus(coleta.status)}>{nomeStatus(coleta.status)}</span>
          </div>
          <div>
            <strong>Prioridade:</strong>{' '}
            {ehPrioridadeAlta(coleta.prioridade) && <FiAlertTriangle className="icone-alta" />}
            {nomePrioridade(coleta.prioridade)}
          </div>
          <div><strong>Motorista:</strong> {coleta.motorista ? coleta.motorista.nome : '-'}</div>
          <div><strong>Veículo:</strong> {coleta.veiculo ? coleta.veiculo.placa : '-'}</div>
          <div className="dados-largo"><strong>Observação:</strong> {coleta.observacao || '-'}</div>
        </div>
        {!cancelada && !coletada && (
          <button className="btn" onClick={() => navigate(`/coletas/${coleta.id}/editar`)}>
            <FiEdit /> Editar
          </button>
        )}
      </div>

      {/* Acoes operacionais */}
      {!cancelada && !coletada && (
        <div className="card">
          <h3>Atribuir motorista e veículo</h3>
          <div className="filtros">
            <div className="campo">
              <label>Motorista</label>
              <select value={motoristaId} onChange={(e) => setMotoristaId(e.target.value)}>
                <option value="">Selecione</option>
                {motoristas.map((m) => (
                  <option key={m.id} value={m.id}>{m.nome}</option>
                ))}
              </select>
            </div>
            <div className="campo">
              <label>Veículo</label>
              <select value={veiculoId} onChange={(e) => setVeiculoId(e.target.value)}>
                <option value="">Selecione</option>
                {veiculos.map((v) => (
                  <option key={v.id} value={v.id}>{v.placa} - {v.modelo}</option>
                ))}
              </select>
            </div>
            <button
              className="btn"
              disabled={!motoristaId || !veiculoId}
              onClick={() => executar(() => atribuirMotoristaVeiculo(coleta.id, Number(motoristaId), Number(veiculoId)))}
            >
              <FiUserPlus /> Atribuir
            </button>
          </div>
        </div>
      )}

      <div className="card">
        <h3>Mudar status</h3>
        <div className="acoes">
          {!cancelada && !coletada && (
            <button
              className="btn"
              disabled={!temMotoristaVeiculo || coleta.status !== 2}
              onClick={() => executar(() => iniciarColeta(coleta.id))}
            >
              <FiPlayCircle /> Em coleta
            </button>
          )}
          {!cancelada && !coletada && (
            <button
              className="btn btn-sucesso"
              disabled={!temMotoristaVeiculo}
              title={!temMotoristaVeiculo ? 'Vincule motorista e veículo primeiro' : ''}
              onClick={() => executar(() => concluirColeta(coleta.id))}
            >
              <FiCheckCircle /> Marcar como coletado
            </button>
          )}
          {!cancelada && !coletada && (
            <button
              className="btn btn-perigo"
              onClick={() => executar(() => cancelarColeta(coleta.id))}
            >
              <FiXCircle /> Cancelar
            </button>
          )}
          {(cancelada || coletada) && (
            <p className="aviso">Esta coleta está {nomeStatus(coleta.status).toLowerCase()} e não permite mais alterações de status.</p>
          )}
        </div>
      </div>

      {/* Ocorrencias */}
      <div className="card">
        <h3>Ocorrências</h3>
        <div className="filtros">
          <div className="campo campo-largo">
            <label>Descrição</label>
            <input
              value={descricaoOcorrencia}
              onChange={(e) => setDescricaoOcorrencia(e.target.value)}
              maxLength={500}
              placeholder="Descreva a ocorrência"
            />
          </div>
          <button
            className="btn"
            disabled={!descricaoOcorrencia.trim()}
            onClick={() => executar(async () => {
              await registrarOcorrencia(coleta.id, descricaoOcorrencia);
              setDescricaoOcorrencia('');
            })}
          >
            <FiPlusCircle /> Registrar
          </button>
        </div>

        <ul className="lista-ocorrencias">
          {coleta.ocorrencias && coleta.ocorrencias.length > 0 ? (
            coleta.ocorrencias.map((o) => (
              <li key={o.id}>
                <span>{o.descricao}</span>
                <small>{formatarData(o.dataHoraRegistro)} — {o.usuarioResponsavel}</small>
              </li>
            ))
          ) : (
            <li className="vazio">Nenhuma ocorrência registrada.</li>
          )}
        </ul>
      </div>
    </div>
  );
}
