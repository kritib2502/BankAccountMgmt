using BankAccountMgmt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankAccountMgmt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<BankAccountType> AccountTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.ClientId, ca.AccountNum });

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.Client)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.ClientId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.BankAccount)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.AccountNum })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BankAccount>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(9,2)");

            modelBuilder.Entity<BankAccount>()
                .Property(p => p.AccountType)
                .HasColumnType("varchar(15)");

            modelBuilder.Entity<BankAccountType>()
               .HasKey(at => new { at.AccountType });

            modelBuilder.Entity<BankAccount>()
                .HasOne(p => p.BankAccountType)
                .WithMany(p => p.BankAccounts)
                .HasForeignKey(fk => new { fk.AccountType })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .Property(p => p.FirstName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.LastName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.Email)
                .HasColumnType("varchar(50)");
        }

    }
}