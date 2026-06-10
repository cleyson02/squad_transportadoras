using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Placa { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        [StringLength(50)]
        public string? Marca { get; set; }

        public decimal CapacidadeCargaKg { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<Coleta>? Coletas { get; set; }
    }
}