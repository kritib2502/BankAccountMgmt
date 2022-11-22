using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using BankAccountMgmt.ViewModels;
using BankAccountMVC.Repositories;

namespace BankAccountMgmt.Repositories
{
    public class BankAccountRepo
    {
        ApplicationDbContext _db;
        public BankAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }


/*------------------------------------------------ TO GET AN ACCOUNT -----------------------------------------------*/
        public BankAccount GetAccount(int accountNum)
        {
            var bankAccount = _db.BankAccounts.FirstOrDefault(x => x.AccountNum == accountNum);
            return bankAccount;
        }

/*------------------------------------------------ CREATING AN ACCOUNT -----------------------------------------------*/

        public Tuple<int,string> CreateAccount(BankAccountVM bankAccountVM, string email)
        {
            string message = "";

            BankAccount bankAccount = new BankAccount();  
            ClientAccount clientAccount = new ClientAccount();

            try
            {

                bankAccount = new BankAccount
                { 
                    AccountType = bankAccountVM.AccountType,
                    Balance = bankAccountVM.Balance,
                };

                _db.BankAccounts.Add(bankAccount);
                _db.SaveChanges();

                ClientRepo clientRepo = new ClientRepo(_db);
                Client client = clientRepo.GetClient(email);

               clientAccount = new ClientAccount
                {
                    ClientId = client.ClientId,
                    AccountNum = bankAccount.AccountNum
                };

                _db.ClientAccounts.Add(clientAccount);
                _db.SaveChanges();

                message = $"Success creating your {bankAccount.AccountType} account, your new account number is {bankAccount.AccountNum}";

            }
            catch (Exception ex)
            {

                bankAccount.AccountNum = -1;
                message = $"Error creating your new account, error: {ex.Message}";
            }

            return Tuple.Create(bankAccount.AccountNum,message);
        }


/*------------------------------------------------ EDITING AN ACCOUNT -----------------------------------------------*/
        public string EditAccount(ClientAccountVM bankAccountVM)
        {
            string message = "";
            try
            {
              
                Client client = new Client();
                BankAccount bankAccount = new BankAccount();

                client = new Client
                {
                    ClientId = bankAccountVM.ClientId,
                    FirstName = bankAccountVM.FirstName,
                    LastName = bankAccountVM.LastName,
                    Email = bankAccountVM.Email,
                };

                _db.Clients.Update(client);
                _db.SaveChanges();

                bankAccount = new BankAccount
                {
                    AccountNum = bankAccountVM.AccountNum,
                    AccountType = bankAccountVM.AccountType,
                    Balance = bankAccountVM.Balance,
                };   

                _db.BankAccounts.Update(bankAccount);
                _db.SaveChanges();

                message = $"Success editing {bankAccount.AccountType} Account No. {bankAccount.AccountNum}";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }


/*------------------------------------------------ DELETING AN ACCOUNT -----------------------------------------------*/


        public BankAccountVM GetBankAccount(int accountNum)
        {
           var bankAccount = _db.BankAccounts.FirstOrDefault(x => x.AccountNum == accountNum);
            BankAccountVM bankAccountVM = new BankAccountVM{
                AccountNum = bankAccount.AccountNum,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance
            };
           
            return bankAccountVM;
        }

        public string DeleteAccount(int accountNum)
        {
            string message = "";
            try
            {
                BankAccountRepo brRepo = new BankAccountRepo(_db);
                BankAccount bankAccount = brRepo.GetAccount(accountNum);


                ClientAccount clientAccount = brRepo.GetClientAccount(accountNum);

                _db.ClientAccounts.Remove(clientAccount);
                _db.SaveChanges();
                _db.BankAccounts.Remove(bankAccount);
                _db.SaveChanges();
                message = "The account has been deleted.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public ClientAccount GetClientAccount(int accountNum)
        {
            var clientAccount = _db.ClientAccounts.FirstOrDefault(x => x.AccountNum == accountNum);
            return clientAccount;
        }

    }
}
