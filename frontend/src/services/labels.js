// Mapeamentos usados em varias telas para traduzir os enums numericos da API.

export const STATUS = {
  1: 'Aberta',
  2: 'Atribuída',
  3: 'Em Coleta',
  4: 'Coletada',
  5: 'Cancelada',
};

export const PRIORIDADE = {
  1: 'Baixa',
  2: 'Normal',
  3: 'Alta',
};

export function nomeStatus(valor) {
  return STATUS[valor] || 'Desconhecido';
}

export function nomePrioridade(valor) {
  return PRIORIDADE[valor] || 'Desconhecida';
}

// Classe CSS do "badge" de status (cores simples).
export function classeStatus(valor) {
  switch (valor) {
    case 1: return 'badge badge-aberta';
    case 2: return 'badge badge-atribuida';
    case 3: return 'badge badge-emcoleta';
    case 4: return 'badge badge-coletada';
    case 5: return 'badge badge-cancelada';
    default: return 'badge';
  }
}

export function ehPrioridadeAlta(valor) {
  return valor === 3;
}

// Formata uma data ISO para o padrao brasileiro.
export function formatarData(iso) {
  if (!iso) return '-';
  const d = new Date(iso);
  return d.toLocaleString('pt-BR');
}
