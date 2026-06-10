using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    // Filtros e paginacao usados na listagem de coletas.
    public class ColetaFiltroDto
    {
        // Filtra por status (numero do enum StatusColeta). Opcional.
        public int? Status { get; set; }

        // Filtra por cliente. Opcional.
        public int? ClienteId { get; set; }

        // Filtro por periodo (data de retirada). Opcionais.
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        // Paginacao simples.
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }

    // Resultado paginado generico devolvido para a listagem.
    public class ResultadoPaginado<T>
    {
        public IEnumerable<T> Itens { get; set; } = new List<T>();
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas { get; set; }
    }

    // Dados enviados para registrar uma ocorrencia.
    public class CriarOcorrenciaDto
    {
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
    }
}
