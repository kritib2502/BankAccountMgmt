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
        /// 1. This method returns the list of bank accounts of the user logged in
        /// Using the ClientAccountVM 
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
        /// 1. This method returns the required account details to be passed to the Details View.
        /// 2. ClientRepo and BankAccount Repo are called to gather information from Client Table and BankAccount Table
        /// </summary>
        /// <param name="accountNum"></param>
        /// <param name="email"></param>
        /// <returns>ClientAccountVM</returns>
        public ClientAccountVM GetAccountDetail(int accountNum, int clientID)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccount bankAccount = bankAccountRepo.GetAccount(accountNum);

            ClientRepo clientRepo = new ClientRepo(_db);
            Client client = clientRepo.GetClient(clientID);


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
