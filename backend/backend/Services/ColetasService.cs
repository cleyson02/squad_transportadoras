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

        public async Task<IEnumerable<Coleta>> GetColetas()
        {
            return await _context.Coletas
                .Include(c => c.Cliente)
                .Include(c => c.Motorista)
                .Include(c => c.Veiculo)
                .ToListAsync();
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

        public async Task<IEnumerable<Coleta>> GetColetasByRemetente(string remetente)
        {
            return await _context.Coletas
                .Where(c => c.Remetente.Contains(remetente))
                .ToListAsync();
        }

        public async Task CreateColeta(Coleta coleta)
        {
            coleta.DataSolicitacao = DateTime.Now;
            coleta.Status = StatusColeta.Aberta;

            await _context.Coletas.AddAsync(coleta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateColeta(Coleta coleta)
        {
            var coletaExistente = await _context.Coletas.FindAsync(coleta.Id);

            if (coletaExistente == null)
                throw new Exception("Coleta não encontrada.");

            if (coletaExistente.Status == StatusColeta.Cancelada)
                throw new Exception(
                    "Coletas canceladas não podem ser alteradas."
                );

            _context.Entry(coletaExistente)
                .CurrentValues
                .SetValues(coleta);

            await _context.SaveChangesAsync();
        }

        public async Task CancelarColeta(int coletaId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId);

            if (coleta == null)
                throw new Exception("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Coletada)
                throw new Exception(
                    "Uma coleta concluída não pode ser cancelada."
                );

            coleta.Status = StatusColeta.Cancelada;

            await _context.SaveChangesAsync();
        }

        public async Task AtribuirMotoristaVeiculo(
            int coletaId,
            int motoristaId,
            int veiculoId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId);

            if (coleta == null)
                throw new Exception("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new Exception(
                    "Não é possível atribuir recursos a uma coleta cancelada."
                );

            coleta.MotoristaId = motoristaId;
            coleta.VeiculoId = veiculoId;
            coleta.Status = StatusColeta.Atribuida;

            await _context.SaveChangesAsync();
        }

        public async Task ConcluirColeta(int coletaId)
        {
            var coleta = await _context.Coletas.FindAsync(coletaId);

            if (coleta == null)
                throw new Exception("Coleta não encontrada.");

            if (coleta.Status == StatusColeta.Cancelada)
                throw new Exception(
                    "Coletas canceladas não podem ser concluídas."
                );

            if (coleta.MotoristaId == null)
                throw new Exception(
                    "É necessário vincular um motorista."
                );

            if (coleta.VeiculoId == null)
                throw new Exception(
                    "É necessário vincular um veículo."
                );

            coleta.Status = StatusColeta.Coletada;

            await _context.SaveChangesAsync();
        }

        public Task<ResultadoPaginado<Coleta>> GetColetas(ColetaFiltroDto filtro)
        {
            throw new NotImplementedException();
        }

        public Task<Coleta> CreateColeta(CriarColetaDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateColeta(int id, AtualizarColetaDto dto)
        {
            throw new NotImplementedException();
        }

        public Task IniciarColeta(int coletaId)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarOcorrencia(int coletaId, string descricao, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}