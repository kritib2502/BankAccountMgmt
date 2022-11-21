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
        public void SaveClient(Client client)
        {
            _db.Clients.Add(client);
            _db.SaveChanges();
        }

        public Client GetClient(string email)
        {
            var client = _db.Clients.FirstOrDefault(x => x.Email == email);
            return client;
        }
        public Client GetClient(int clientId)
        {
            var client = _db.Clients.FirstOrDefault(x => x.ClientId == clientId);
            return client;
        }
    }
}
