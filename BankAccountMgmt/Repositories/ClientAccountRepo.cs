using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using BankAccountMgmt.ViewModels;
using BankAccountMVC.Repositories;
using System.Linq;

namespace BankAccountMgmt.Repositories
{
    public class ClientAccountRepo
    {
        ApplicationDbContext _db;
        public ClientAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List<ClientAccountVM></returns>
        public List<ClientAccountVM> GetClientAccountsByEmail(string email)
        {
            ClientRepo clientRepo = new ClientRepo(_db);
            Client client = clientRepo.GetClient(email);

            IEnumerable<int> accountNumbers = _db.ClientAccounts.Where(ca => ca.ClientId == client.ClientId).Select(ca => ca.AccountNum);

            var bankAccounts = _db.BankAccounts.Where(ba => accountNumbers.Contains(ba.AccountNum)).OrderByDescending(ba => ba.AccountNum);

            List<ClientAccountVM> clientAccountVMs = new List<ClientAccountVM>();

            foreach (var account in bankAccounts)
            {
                clientAccountVMs.Add(new ClientAccountVM
                {
                    AccountNum = account.AccountNum,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    AccountType = account.AccountType,
                });
            }
            return clientAccountVMs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNum"></param>
        /// <param name="email"></param>
        /// <returns>ClientAccountVM</returns>
        public ClientAccountVM GetAccountDetail(int accountNum, string email)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccount bankAccount = bankAccountRepo.GetAccount(accountNum);

            ClientRepo clientRepo = new ClientRepo(_db);
            Client client = clientRepo.GetClient(email);


            ClientAccountVM accountDetail = new ClientAccountVM
            {
                ClientId = client.ClientId,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                AccountNum = bankAccount.AccountNum,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance
            };

            return accountDetail;
        }

    }

}
