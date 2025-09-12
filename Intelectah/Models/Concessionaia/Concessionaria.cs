using System.ComponentModel.DataAnnotations;

namespace Intelectah.Models
{
    public class Concessionaria
    {
        public int ConcessionariaID { get; set; }

        [Required]
        [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
    public string Endereco { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
    public string Email { get; set; } = string.Empty;

        // Campo para deleção lógica
        public bool IsAtivo { get; set; } = true;
    }
}