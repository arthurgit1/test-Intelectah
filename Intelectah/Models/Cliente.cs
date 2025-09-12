using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Intelectah.Models
{
    public class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var cpf = value.ToString();
            if (!IsCpfValid(cpf))
                return new ValidationResult("CPF inválido.");
            return ValidationResult.Success;
        }

        // Validação de CPF (apenas formato e dígitos)
        public static bool IsCpfValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = Regex.Replace(cpf, "[^0-9]", "");
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;
            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;
            for (int i = 0; i < 9; i++) sum += int.Parse(tempCpf[i].ToString()) * mult1[i];
            int rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            string digit = rest.ToString();
            tempCpf += digit;
            sum = 0;
            for (int i = 0; i < 10; i++) sum += int.Parse(tempCpf[i].ToString()) * mult2[i];
            rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            digit += rest.ToString();
            return cpf.EndsWith(digit);
        }
    }

    public class Cliente
    {
        public int ClienteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        [Cpf]
        public string CPF { get; set; }

        [Required]
        [StringLength(100)]
        public string Endereco { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        // Campo para deleção lógica
        public bool Ativo { get; set; } = true;
    }
}