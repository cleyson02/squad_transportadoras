// Mapeamentos perfeitamente alinhados com o backend C#
export const STATUS = {
  1: 'Aberta',
  3: 'Atribuída',
  4: 'Em Coleta',
  5: 'Coletada',
  6: 'Cancelada',
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

// Atualize as cores para não quebrar os novos números
export function classeStatus(valor) {
  switch (Number(valor)) {
    case 1: return 'badge badge-aberta';
    case 3: return 'badge badge-atribuida';
    case 4: return 'badge badge-emcoleta';
    case 5: return 'badge badge-coletada';
    case 6: return 'badge badge-cancelada';
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
