using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAccountMgmt.Models
{
    public class Client
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        [Required]
        public int ClientId { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [Required]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [Required]
        public string LastName { get; set; }

        [Editable(false)]
        [Required]
        public string Email { get; set; }
        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }
    }
}
