using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Motorista
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<Coleta>? Coletas { get; set; }
    }
}