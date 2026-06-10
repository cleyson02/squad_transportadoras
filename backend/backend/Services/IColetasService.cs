using backend.Models;

namespace backend.Services
{
    public interface IColetasService
    {
        Task<IEnumerable<Coleta>> GetColetas();

        Task<Coleta?> GetColeta(int id);

        Task<IEnumerable<Coleta>> GetColetasByRemetente(string remetente);

        Task CreateColeta(Coleta coleta);

        Task UpdateColeta(Coleta coleta);

        Task AtribuirMotoristaVeiculo(
            int coletaId,
            int motoristaId,
            int veiculoId);

        Task CancelarColeta(int coletaId);

        Task ConcluirColeta(int coletaId);
    }
}