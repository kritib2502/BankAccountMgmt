using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAccountMgmt.ViewModels
{
    public class BankAccountVM
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNum { get; set; }

        public string AccountType { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$|^[0-9]+.[0-9]{1}$|^[0-9]+.[0-9]{2}$", ErrorMessage = "Positive numbers only")]
        public decimal Balance { get; set; }

    }
}
