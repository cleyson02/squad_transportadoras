import api from './api';

// Faz login na API e devolve os dados do usuario + token.
export async function login(usuario, senha) {
  const resposta = await api.post('/auth/login', { usuario, senha });
  return resposta.data;
}
