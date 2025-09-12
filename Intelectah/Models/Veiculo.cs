using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intelectah.Models
{
    public class Veiculo
    {
        public int VeiculoID { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        [Required]
        [StringLength(50)]
        public string Cor { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Ano { get; set; }

        [Required]
        [StringLength(20)]
        public string Placa { get; set; }

        [Required]
        public decimal Preco { get; set; }

        // Relacionamento com Fabricante
        [Display(Name = "Fabricante")]
        public int FabricanteID { get; set; }
        public Fabricante Fabricante { get; set; }
    }
}