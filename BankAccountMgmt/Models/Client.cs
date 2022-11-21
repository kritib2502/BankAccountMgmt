using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAccountMgmt.Models
{
    public class Client
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; private set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        public string LastName { get; set; }

        [Editable(false)]
        public string Email { get; set; }
        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }
    }
}
