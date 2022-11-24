using BankAccountMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankAccountMgmt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the landing page containing description about the Bank and its statement
        /// </summary>
        /// <returns> IActionResult </returns>
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Displays the privacy page
        /// </summary>
        /// <returns> IActionResult </returns>
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}