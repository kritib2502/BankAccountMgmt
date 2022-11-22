using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAccountMgmt.Models
{
    public class BankAccount
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public int AccountNum { get; set; }

        public string? AccountType { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$|^[0-9]+.[0-9]{1}$|^[0-9]+.[0-9]{2}$", ErrorMessage = "Positive numbers only")]
        public decimal Balance { get; set; }
        public virtual ICollection<ClientAccount>? ClientAccounts { get; set; }
        public virtual BankAccountType BankAccountType { get; set; }
    }
}
