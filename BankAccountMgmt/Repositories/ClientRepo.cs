using BankAccountMgmt.Data;
using BankAccountMgmt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankAccountMVC.Repositories
{
    public class ClientRepo
    {
        ApplicationDbContext _db;
        public ClientRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// 1. This method returns a client record based on the email provided 
        ///  of the logged in user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Client </returns>
        public Client GetClient(string email)
        {
            var client = _db.Clients.FirstOrDefault(x => x.Email == email);
            return client;
        }

        /// <summary>
        /// 1. This method returns a client record based on the client id provided 
        ///  of the logged in user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns> Client </returns>
        public Client GetClient(int clientId)
        {
            var client = _db.Clients.FirstOrDefault(x => x.ClientId == clientId);
            return client;
        }
    }
}
