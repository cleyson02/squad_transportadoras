using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Models
{
    public class Coleta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroSolicitacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Remetente { get; set; }

        [Required]
        [StringLength(100)]
        public string Destinatario { get; set; }

        [Required]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        [Required]
        public DateTime DataRetirada { get; set; }

        [Required]
        public Prioridade Prioridade { get; set; }

        [Required]
        public StatusColeta Status { get; set; } = StatusColeta.Aberta;

        [StringLength(500)]
        public string? Observacao { get; set; }

        public int? MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public Motorista? Motorista { get; set; }

        public int? VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo? Veiculo { get; set; }

        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        public ICollection<Ocorrencia>? Ocorrencias { get; set; }
    }
}