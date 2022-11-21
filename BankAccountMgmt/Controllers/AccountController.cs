using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using BankAccountMgmt.Repositories;
using BankAccountMgmt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index()
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
            IEnumerable<ClientAccountVM> accounts = clientAccountRepo.GetClientAccountsByEmail(User.Identity.Name);
            return View(accounts);
        }

/*------------------------------------------------ DETAILS-----------------------------------------------*/
        public IActionResult Details(int num, string message)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);

            ClientAccountVM detail = clientAccountRepo.GetAccountDetail(num, User.Identity.Name);

            ViewData["message"] = message;
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

        public IActionResult Create([Bind("AccountNum,AccountType,Balance")] BankAccount bankAccount)
        {
            ViewData["message"] = "";
            string createMsg = " ";
            int accountNum = 0;

            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                accountNum = bankAccountRepo.CreateAccount(bankAccount, User.Identity.Name);
                createMsg = $"Success creating your {bankAccount.AccountType} account, your new account number is {bankAccount.AccountNum}";


                return RedirectToAction("Details", "Account", new { num = bankAccount.AccountNum, message = createMsg });

                //ViewData["message"] = createMsg;
            }

            ViewData["message"] = "Invalid form data, please try again.";

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
        public IActionResult Edit([Bind("AccountNum,AccountType,Balance")] BankAccount bankAccount)
        {
            ViewData["message"] = "";
            string editMsg = " ";

            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                editMsg = bankAccountRepo.EditAccount(bankAccount);

                return RedirectToAction("Details", "Account", new { num = bankAccount.AccountNum, message = editMsg });
            }

            ViewData["message"] = "Invalid form data, please try again.";

            ViewData["AccountType"] = new SelectList(_db.AccountTypes, "AccountType", "AccountType", bankAccount.AccountType);

            return View(bankAccount);
        }

/*------------------------------------------------ DELETE -----------------------------------------------*/
        public IActionResult Delete(int id)
        {
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
            BankAccount account = bankAccountRepo.GetAccountDetail(id, User.Identity.Name);
            return View(account);
        }

        [HttpPost] // POST: Account/Delete
        public IActionResult Delete([Bind("AccountNum,AccountType,Balance")] BankAccount account)
        {

            ViewData["message"] = "";
            string deleteMsg = " ";
            if (ModelState.IsValid)
            {
                ModelState.Remove("ClientAccount");
                // ModelState.ClearValidationState("ClientAccount");
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);
                deleteMsg = bankAccountRepo.DeleteAccount(account.AccountNum);

                ViewData["message"] = deleteMsg;
                return RedirectToAction("Index", "Account", new { message = deleteMsg });
            }

            ViewData["message"] = "Invalid form data, please try again.";

            return View(account);
        }



    }
}
