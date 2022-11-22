using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using BankAccountMgmt.Repositories;
using BankAccountMgmt.ViewModels;
using BankAccountMVC.Repositories;
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


        public IActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ClientRepo clientRepo = new ClientRepo(_db);
            HttpContext.Session.SetString("FirstName", clientRepo.GetClient(User.Identity.Name).FirstName);


            //HttpContext.Session.SetString("Client", clientRepo.GetClient(User.Identity.Name).ClientId);

            if (string.IsNullOrEmpty(sortOrder))
            {
                ViewData["accountNumSortParm"] = "accountNum_desc";
            }
            else
            {
                ViewData["accountNumSortParm"] = sortOrder == "AccountNum" ? "accountNum_desc" : "AccountNum";
            }

            ViewData["accountTypeSortParm"] = sortOrder == "AccountType" ? "accountType_desc" : "accountType";

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
                    accounts = accounts.OrderByDescending(a => a.AccountType);
                    break;
                case "AccountNum":
                    accounts = accounts.OrderBy(a => a.AccountType);
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
            public IActionResult Details(int num, string message)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);

            ClientAccountVM detail = clientAccountRepo.GetAccountDetail(num, User.Identity.Name);

             detail.Message = message;

            return View(detail);
        }


/*------------------------------------------------ CREATE -----------------------------------------------*/
        // GET: Account/Create

        public IActionResult Create()
        {
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType");
            return View();
        }

        [HttpPost] // POST: Account/Create

        public IActionResult Create([Bind("AccountNum,AccountType,Balance")] BankAccountVM bankAccount)
        {
            bankAccount.Message = "Invalid entry please try again";
            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                Tuple<int,string> response = bankAccountRepo.CreateAccount(bankAccount, User.Identity.Name);


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
        public IActionResult Edit(int id)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
            ClientAccountVM detail = clientAccountRepo.GetAccountDetail(id, User.Identity.Name);
            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType");
            return View(detail);
        }

        [HttpPost] // POST: Account/Edit
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
        public IActionResult Delete(int id)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccountVM account = bankAccountRepo.GetBankAccount(id);
            return View(account);
        }

        [HttpPost] // POST: Account/Delete
        public IActionResult Delete([Bind("AccountNum,AccountType,Balance")] BankAccountVM  deleteAccount)
        {

            
            deleteAccount.Message = "Invalid form data, please try again.";
            string deleteMsg = " ";
            if (ModelState.IsValid)
            {
                //ModelState.Remove("ClientAccount");
                // ModelState.ClearValidationState("ClientAccount");
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                deleteMsg = bankAccountRepo.DeleteAccount(deleteAccount.AccountNum);

                deleteAccount.Message = deleteMsg;
                return RedirectToAction("Index", "Account", new { message = deleteMsg });
            }


            return View(deleteAccount);
        }



    }
}
