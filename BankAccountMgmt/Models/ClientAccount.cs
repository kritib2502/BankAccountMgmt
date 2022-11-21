namespace BankAccountMgmt.Models
{
    public class ClientAccount
    {
        public int ClientId { get; set; }

        public int AccountNum { get; set; }

        public virtual Client Client { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
