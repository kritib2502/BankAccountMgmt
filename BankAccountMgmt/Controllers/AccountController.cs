using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using BankAccountMgmt.Repositories;
using BankAccountMgmt.ViewModels;
using BankAccountMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;


namespace BankAccountMgmt.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;


        public AccountController(ApplicationDbContext context)
        {
            _db = context;
        }

  /*------------------------------------------------ INDEX -----------------------------------------------*/
  /// <summary>
  /// 1. Passing the list of accounts from the View Model to the Index View
  /// 2. Using Functionalities: Search on Account Type, Sort on Account Type and Account Number
  /// 3. Using Pagination on the Index View 
  /// 4. Client Account Repostiory is used to call a method to return list of accounts using the ClientID of the logged in user.
  /// </summary>
  /// <param name="sortOrder"></param>
  /// <param name="searchString"></param>
  /// <param name="currentFilter"></param>
  /// <param name="page"></param>
  /// <returns> IActionResult </returns>
 
        [Authorize]
  
        public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? page,string message)
        {
            ClientRepo clientRepo = new ClientRepo(_db);
            HttpContext.Session.SetString("FirstName", clientRepo.GetClient(User.Identity.Name).FirstName);

            HttpContext.Session.SetString("ClientID",Convert.ToString(clientRepo.GetClient(User.Identity.Name).ClientId));


            ViewData["Message"] = message;
            if (string.IsNullOrEmpty(sortOrder))
            {
                ViewData["accountNumSortParm"] = "accountNum_desc";
            }
            else
            {
                ViewData["accountNumSortParm"] = sortOrder == "AccountNum" ? "accountNum_desc" : "AccountNum";
            }

            ViewData["accountTypeSortParm"] = sortOrder == "AccountType" ? "accountType_desc" : "AccountType";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
            IEnumerable<ClientAccountVM> accounts = clientAccountRepo.GetClientAccountsByEmail(User.Identity.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(a => a.AccountType.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "accountNum_desc":
                    accounts = accounts.OrderByDescending(a => a.AccountNum);
                    break;
                case "AccountNum":
                    accounts = accounts.OrderBy(a => a.AccountNum);
                    break;
                case "accountType_desc":
                    accounts = accounts.OrderByDescending(a => a.AccountType);
                    break;
                default:
                    accounts = accounts.OrderByDescending(a => a.AccountNum);
                    break;
            }

            int pageSize = 2;
            return View(PaginatedList<ClientAccountVM>.Create(accounts.AsQueryable().AsNoTracking(), page ?? 1, pageSize));
        }

  /*------------------------------------------------ DETAILS-----------------------------------------------*/

        /// <summary>
        /// 1. Calling the client account repo to get the details of the account
        ///  from client account view model using the account number and client id.
        /// 2. Passing the account details to the view
        /// </summary>
        /// <param name="num"></param>
        /// <param name="message"></param>
        /// <returns> IActionResult </returns>
            public IActionResult Details(int num, string message)
            {
              int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));
              ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
              ClientAccountVM detail = clientAccountRepo.GetAccountDetail(num, clientID);

              detail.Message = message;
               return View(detail);
             }

 /*------------------------------------------------ CREATE -----------------------------------------------*/

        /// <summary>
        /// 1. GET: Account/Create
        /// </summary>
        /// <returns> IActionResult </returns>
        public IActionResult Create()
        {
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType");
            return View();
        }

        /// <summary>
        /// POST: Account/Create 
        /// 1. Binding data from the BankAccountVM
        /// 2. Calling the bankAccountRepo to create client account and bank account
        /// 3. Passing the account number and appropriate message to the details view 
        ///    once account creation is successful.
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns> IActionResult </returns>
        /// 
        [HttpPost] 
       

        public IActionResult Create([Bind("AccountNum,AccountType,Balance")] BankAccountVM bankAccount)
        {
            ClientRepo clientRepo = new ClientRepo(_db);
            HttpContext.Session.SetString("ClientID", Convert.ToString(clientRepo.GetClient(User.Identity.Name).ClientId));
            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));
            bankAccount.Message = "Invalid form data, please try again";

            if (ModelState.IsValid)
            {               
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                Tuple<int,string> response = bankAccountRepo.CreateAccount(bankAccount, clientID);

                if (response.Item1 < 0)
                {
                    bankAccount.Message = response.Item2;
                }
                else
                {
                    return RedirectToAction("Details", "Account", new { num = response.Item1,message = response.Item2 });
                }
              
            }
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType", bankAccount.AccountType);
            return View(bankAccount);
        }

        /*------------------------------------------------ EDIT -----------------------------------------------*/
        /// <summary>
        /// GET: Account/Edit
        /// 1. Calling the clientAccountRepo to display the details of the account by passing the account number and client id.
        /// 2. Using the AccountType table to display a dropdown list of the various types of accounts.
        /// 3. Passes the detail of the selected account we want to modify to the view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> IActionResult </returns>
  
        public IActionResult Edit(int id)
        {          
            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
            ClientAccountVM detail = clientAccountRepo.GetAccountDetail(id, clientID);
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType");
            return View(detail);
        }

        /// <summary>
        /// POST: Account/Edit
        /// 1. This method uses the CientAccountVM to modify the selected account
        /// 2. Passes the account number and appropriate message back to the details view 
        ///    after the modification is successful
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns> IActionResult </returns>


        [HttpPost] 
        public IActionResult Edit([Bind("ClientId,FirstName,LastName,Email,AccountNum,AccountType,Balance")] ClientAccountVM bankAccountVM)
        {
            bankAccountVM.Message = "Invalid form data, please try again.";
            string editMsg = " ";

            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                editMsg = bankAccountRepo.EditAccount(bankAccountVM);

                return RedirectToAction("Details", "Account", new { num = bankAccountVM.AccountNum, message = editMsg });
            }
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType", bankAccountVM.AccountType);
            return View(bankAccountVM);
        }

/*------------------------------------------------ DELETE -----------------------------------------------*/
/// <summary>
/// GET: Account/Delete
/// 1. This method calls the bankAccountRepo to get the Bank Account we wish to delete using the account number.
/// 2. Using the BankAccountVM to pass the account information to the view
/// </summary>
/// <param name="id"></param>
/// <returns> IActionResult </returns>

        public IActionResult Delete(int id)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccountVM account = bankAccountRepo.GetBankAccount(id);
            return View(account);
        }

 /// <summary>
 /// POST: Account/Delete 
 /// 1. Using the BankAccountVM to remove ClientAccount and BankAccount by parsing them separately.
 /// 2. BankAccountRepo is calling the method to delete the respective accounts.
 /// 3. Returning a valid message when redirecting to the Index View after deletion is successful.
 /// </summary>
 /// <param name="deleteAccount"></param>
 /// <returns> IActionResult </returns>
 
        [HttpPost] 
        public IActionResult Delete([Bind("AccountNum,AccountType,Balance")] BankAccountVM  deleteAccount)
        {           
            deleteAccount.Message = "Invalid form data, please try again.";
            string deleteMsg = " ";
            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                deleteMsg = bankAccountRepo.DeleteAccount(deleteAccount.AccountNum);

                deleteAccount.Message = deleteMsg;
                return RedirectToAction("Index", "Account", new { message = deleteMsg });          
            }
            return View(deleteAccount);
        }

    }
}
