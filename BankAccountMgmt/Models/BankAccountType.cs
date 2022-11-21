using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BankAccountMgmt.Models
{
    public class BankAccountType
    {

        [Key]
        public string AccountType { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}
