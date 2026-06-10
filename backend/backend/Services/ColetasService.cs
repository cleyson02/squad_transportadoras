using backend.Context;
using backend.Dtos;
using backend.Enums;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ColetasService : IColetasService
    {
        private readonly AppDbContext _context;

        public ColetasService(AppDbContext context)
        {
            _context = context;
        }

        // Listagem com filtros (status, cliente, periodo) e paginacao simples.
        public async Task<ResultadoPaginado<Coleta>> GetColetas(ColetaFiltroDto filtro)
        {
            var query = _context.Coletas
                .Include(c => c.Cliente)
                .Include(c => c.Motorista)
                .Include(c => c.Veiculo)
                .AsQueryable();

            if (filtro.Status.HasValue)
                query = query.Where(c => (int)c.Status == filtro.Status.Value);

            if (filtro.ClienteId.HasValue)
                query = query.Where(c => c.ClienteId == filtro.ClienteId.Value);

            if (filtro.DataInicio.HasValue)
                query = query.Where(c => c.DataRetirada >= filtro.DataInicio.Value);

            if (filtro.DataFim.HasValue)
                query = query.Where(c => c.DataRetirada <= filtro.DataFim.Value);

            // Prioridade alta primeiro, depois as mais recentes.
            query = query
                .OrderByDescending(c => c.Prioridade)
                .ThenByDescending(c => c.DataSolicitacao);

            var totalItens = await query.CountAsync();

            var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
            var tamanho = filtro.TamanhoPagina < 1 ? 10 : filtro.TamanhoPagina;

            var itens = await query
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            return new ResultadoPaginado<Coleta>
            {
                Itens = itens,
                Pagina = pagina,
                TamanhoPagina = tamanho,
                TotalItens = totalItens,
                TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanho)
            };
        }

        public async Task<Coleta?> GetColeta(int id)
        {
            return await _context.Coletas
                .Include(c => c.Cliente)
                .Include(c => c.Motorista)
                .Include(c => c.Veiculo)
                .Include(c => c.Ocorrencias)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coleta> CreateColeta(CriarColetaDto dto)
        {
            var coleta = new Coleta
            {
                Remetente = dto.Remetente,
                Destinatario = dto.Destinatario,
                DataRetirada = dto.DataRetirada,
                Prioridade = dto.Prioridade,
                Observacao = dto.Observacao,
                ClienteId = dto.ClienteId,
                DataSolicitacao = DateTime.Now,
                Status = StatusColeta.Aberta,
                NumeroSolicitacao = await GerarNumeroSolicitacao()
            };

            await _context.Coletas.AddAsync(coleta);
            await _context.SaveChangesAsync();

            return coleta;
        }

        public async Task UpdateColeta(int id, AtualizarColetaDto dto)
        {
            var coleta = await _context.Coletas.FindAsync(id)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new RegraDeNegocioException("Coletas canceladas não podem ser alteradas.");

            coleta.Remetente = dto.Remetente;
            coleta.Destinatario = dto.Destinatario;
            coleta.DataRetirada = dto.DataRetirada;
            coleta.Prioridade = dto.Prioridade;
            coleta.Observacao = dto.Observacao;
            coleta.ClienteId = dto.ClienteId;

            await _context.SaveChangesAsync();
        }

        public async Task CancelarColeta(int coletaId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            // Regra: coleta ja coletada nao pode ser cancelada.
            if (coleta.Status == StatusColeta.Coletada)
                throw new RegraDeNegocioException("Uma coleta já coletada não pode ser cancelada.");

            coleta.Status = StatusColeta.Cancelada;
            await _context.SaveChangesAsync();
        }

        public async Task AtribuirMotoristaVeiculo(int coletaId, int motoristaId, int veiculoId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new RegraDeNegocioException("Não é possível atribuir recursos a uma coleta cancelada.");

            if (coleta.Status == StatusColeta.Coletada)
                throw new RegraDeNegocioException("Não é possível alterar uma coleta já coletada.");

            var motorista = await _context.Motoristas.FindAsync(motoristaId)
                ?? throw new RegraDeNegocioException("Motorista não encontrado.");

            var veiculo = await _context.Veiculos.FindAsync(veiculoId)
                ?? throw new RegraDeNegocioException("Veículo não encontrado.");

            coleta.MotoristaId = motorista.Id;
            coleta.VeiculoId = veiculo.Id;
            coleta.Status = StatusColeta.Atribuida;

            await _context.SaveChangesAsync();
        }

        // Avanca a coleta para "Em coleta" (motorista a caminho / coletando).
        public async Task IniciarColeta(int coletaId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new RegraDeNegocioException("Uma coleta cancelada não pode ir para Em Coleta.");

            if (coleta.MotoristaId == null || coleta.VeiculoId == null)
                throw new RegraDeNegocioException("É necessário atribuir motorista e veículo antes de iniciar a coleta.");

            coleta.Status = StatusColeta.EmColeta;
            await _context.SaveChangesAsync();
        }

        public async Task ConcluirColeta(int coletaId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new RegraDeNegocioException("Coletas canceladas não podem ser coletadas.");

            // Regra: nao pode marcar como coletada sem motorista e veiculo.
            if (coleta.MotoristaId == null)
                throw new RegraDeNegocioException("É necessário vincular um motorista antes de marcar como coletado.");

            if (coleta.VeiculoId == null)
                throw new RegraDeNegocioException("É necessário vincular um veículo antes de marcar como coletado.");

            coleta.Status = StatusColeta.Coletada;
            await _context.SaveChangesAsync();
        }

        // Registra uma ocorrencia guardando data/hora e o usuario responsavel.
        public async Task RegistrarOcorrencia(int coletaId, string descricao, string usuario)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId)
                ?? throw new RegraDeNegocioException("Coleta não encontrada.");

            var ocorrencia = new Ocorrencia
            {
                ColetaId = coleta.Id,
                Descricao = descricao,
                DataHoraRegistro = DateTime.Now,
                UsuarioResponsavel = usuario
            };

            await _context.Ocorrencias.AddAsync(ocorrencia);
            await _context.SaveChangesAsync();
        }

        // Gera um numero unico simples no formato COL-YYYYMMDD-XXXX.
        private async Task<string> GerarNumeroSolicitacao()
        {
            var prefixo = $"COL-{DateTime.Now:yyyyMMdd}-";
            var quantidadeHoje = await _context.Coletas
                .CountAsync(c => c.NumeroSolicitacao.StartsWith(prefixo));

            var sequencial = (quantidadeHoje + 1).ToString("D4");
            return prefixo + sequencial;
        }
    }
}
