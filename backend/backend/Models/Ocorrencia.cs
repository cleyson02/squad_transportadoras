using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Ocorrencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ColetaId { get; set; }

        [ForeignKey("ColetaId")]
        public Coleta Coleta { get; set; }

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; }

        [Required]
        public DateTime DataHoraRegistro { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string UsuarioResponsavel { get; set; }
    }
}