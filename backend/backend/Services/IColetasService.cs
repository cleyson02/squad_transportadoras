using backend.Dtos;
using backend.Models;

namespace backend.Services
{
    public interface IColetasService
    {
        Task<ResultadoPaginado<Coleta>> GetColetas(ColetaFiltroDto filtro);

        Task<Coleta?> GetColeta(int id);

        Task<Coleta> CreateColeta(CriarColetaDto dto);

        Task UpdateColeta(int id, AtualizarColetaDto dto);

        Task AtribuirMotoristaVeiculo(int coletaId, int motoristaId, int veiculoId);

        Task IniciarColeta(int coletaId);

        Task CancelarColeta(int coletaId);

        Task ConcluirColeta(int coletaId);

        Task RegistrarOcorrencia(int coletaId, string descricao, string usuario);
    }
}
