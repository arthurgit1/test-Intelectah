using System;
using System.ComponentModel.DataAnnotations;

namespace Intelectah.Models
{
    public class Venda
    {
        public int VendaID { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [Display(Name = "Veículo")]
        public int VeiculoID { get; set; }
        public Veiculo Veiculo { get; set; }

        [Required]
        [Display(Name = "Concessionária")]
        public int ConcessionariaID { get; set; }
        public Concessionaria Concessionaria { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Venda")]
        public DateTime DataVenda { get; set; }

        [Required]
        [Display(Name = "Valor da Venda")]
        public decimal Valor { get; set; }
    }
}