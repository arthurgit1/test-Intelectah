using System.ComponentModel.DataAnnotations;

namespace Intelectah.Models
{
    public class Fabricante
    {
        public int FabricanteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string PaisOrigem { get; set; }

        [Required]
        [Range(1800, 2100, ErrorMessage = "Ano de fundação deve ser válido e no passado.")]
        public int AnoFundacao { get; set; }

        [Url]
        [StringLength(255)]
        public string Website { get; set; }

        public bool Ativo { get; set; } = true; // Deleção lógica
    }
}