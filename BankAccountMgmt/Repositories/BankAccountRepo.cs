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
/// <summary>
/// 1. This method return a BankAccount record for the account number provided 
///    based on the user logged in.
/// </summary>
/// <param name="accountNum"></param>
/// <returns> BankAccount </returns>
        public BankAccount GetAccount(int accountNum)
        {
            var bankAccount = _db.BankAccounts.FirstOrDefault(x => x.AccountNum == accountNum);
            return bankAccount;
        }

/*------------------------------------------------ CREATING AN ACCOUNT -----------------------------------------------*/
/// <summary>
/// 1. This method uses the BankAccountVM to parse through the ClientAccount and BankAccount Tables
///    to create a new account in the database.
/// 2. Returns the account number of the account created and a message to be displayed once account creation is successful.
/// </summary>
/// <param name="bankAccountVM"></param>
/// <param name="email"></param>
/// <returns> Tuple<int,string> </returns>

        public Tuple<int,string> CreateAccount(BankAccountVM bankAccountVM, int clientID)
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
                Client client = clientRepo.GetClient(clientID);

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

 /// <summary>
 /// 1. This method uses the ClientAccountVM to parse through the Client and BankAccount Tables
 ///    and modify the account details in the database.
 /// 2. Returns a message to be displayed on the Details View once account modification is successful.
 /// </summary>
 /// <param name="bankAccountVM"></param>
/// <returns> string </returns>
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
/// <summary>
/// This method is used to return a bankaccount record through the BankAccountVM using 
/// the account id of the logged in user.
/// </summary>
/// <param name="accountNum"></param>
/// <returns> BankAccountVM </returns>


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

 /// <summary>
 /// This method deletes ClientAccount and BankAccounts from the database
 /// by calling the BankAccount Repository and returns a message to be passed to the Index View
 /// upon successful deletion of the account.
 /// </summary>
 /// <param name="accountNum"></param>
/// <returns>string </returns>
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


 /// <summary>
 /// This method return the ClientAccount Record of the provided account number
 /// of the logged in user.
 /// </summary>
 /// <param name="accountNum"></param>
 /// <returns> ClientAccount </returns>
        public ClientAccount GetClientAccount(int accountNum)
        {
            var clientAccount = _db.ClientAccounts.FirstOrDefault(x => x.AccountNum == accountNum);
            return clientAccount;
        }

    }
}
