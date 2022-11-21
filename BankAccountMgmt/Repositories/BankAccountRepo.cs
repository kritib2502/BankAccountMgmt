using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
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

        public int CreateAccount(BankAccount bankAccount, string email)
        {
            string message = "";
            int accountnum = 0;

            try
            {

                _db.BankAccounts.Add(bankAccount);
                _db.SaveChanges();
                accountnum = bankAccount.AccountNum;

                ClientRepo clientRepo = new ClientRepo(_db);
                Client client = clientRepo.GetClient(email);

                ClientAccount clientAccount = new ClientAccount
                {
                    ClientId = client.ClientId,
                    AccountNum = bankAccount.AccountNum
                };

                _db.ClientAccounts.Add(clientAccount);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return accountnum;
        }


/*------------------------------------------------ EDITING AN ACCOUNT -----------------------------------------------*/
        public string EditAccount(BankAccount bankAccount)
        {
            string message = "";
            try
            {
                _db.BankAccounts.Update(bankAccount);
                _db.SaveChanges();
                message = "Update has been saved.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

 /*------------------------------------------------ GETTING ACCOUNT DETAILS -----------------------------------------------*/
        public BankAccount GetAccountDetail(int accountNum, string email)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccount bankAccount = bankAccountRepo.GetAccount(accountNum);

            ClientRepo clientRepo = new ClientRepo(_db);
            Client client = clientRepo.GetClient(email);


            BankAccount account = new BankAccount
            {
                AccountNum = bankAccount.AccountNum,
                AccountType = bankAccount.AccountType,
                Balance = bankAccount.Balance

            };

            return account;

        }

/*------------------------------------------------ DELETING AN ACCOUNT -----------------------------------------------*/
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
