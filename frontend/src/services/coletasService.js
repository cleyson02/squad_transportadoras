import api from './api';

// Funcoes que conversam com a API de coletas.
// Centraliza as chamadas para manter os componentes simples.

export function listarColetas(filtros) {
  return api.get('/coletas', { params: filtros });
}

export function obterColeta(id) {
  return api.get(`/coletas/${id}`);
}

export function criarColeta(dados) {
  return api.post('/coletas', dados);
}

export function atualizarColeta(id, dados) {
  return api.put(`/coletas/${id}`, dados);
}

export function atribuirMotoristaVeiculo(id, motoristaId, veiculoId) {
  return api.patch(`/coletas/${id}/atribuir`, { motoristaId, veiculoId });
}

export function iniciarColeta(id) {
  return api.patch(`/coletas/${id}/iniciar`);
}

export function concluirColeta(id) {
  return api.patch(`/coletas/${id}/concluir`);
}

export function cancelarColeta(id) {
  return api.patch(`/coletas/${id}/cancelar`);
}

export function registrarOcorrencia(id, descricao) {
  return api.post(`/coletas/${id}/ocorrencias`, { descricao });
}

// Cadastros de apoio (para preencher selects).
export function listarClientes() {
  return api.get('/cadastros/clientes');
}

export function listarMotoristas() {
  return api.get('/cadastros/motoristas');
}

export function listarVeiculos() {
  return api.get('/cadastros/veiculos');
}
