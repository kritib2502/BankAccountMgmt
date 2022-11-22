namespace BankAccountMgmt.ViewModels
{
    public class ClientAccountVM
    {
        public int ClientId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int AccountNum { get; set; }

        public string AccountType { get; set; }
        public decimal Balance { get; set; }

        public string? Message { get; set; }

    }
}
