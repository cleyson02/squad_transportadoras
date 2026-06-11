using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.Dtos
{
    // Dados que o frontend envia para criar uma coleta.
    public class CriarColetaDto
    {
        [Required]
        [StringLength(100)]
        public string Remetente { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destinatario { get; set; } = string.Empty;

        [Required]
        public DateTime DataRetirada { get; set; }

        [Required]
        public Prioridade Prioridade { get; set; }

        [StringLength(500)]
        public string? Observacao { get; set; }

        public int? ClienteId { get; set; }
    }

    // Dados que o frontend envia para editar uma coleta existente.
    public class AtualizarColetaDto
    {
        [Required]
        [StringLength(100)]
        public string Remetente { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destinatario { get; set; } = string.Empty;

        [Required]
        public DateTime DataRetirada { get; set; }

        [Required]
        public Prioridade Prioridade { get; set; }

        [StringLength(500)]
        public string? Observacao { get; set; }

        public int? ClienteId { get; set; }
    }
}
