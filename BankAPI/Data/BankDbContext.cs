using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.AccessControl;
using System.Text;

namespace BankAPI.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions options) : base(options)
        {
        }
        public override int SaveChanges()
        {
            var auditEntries = OnBeforeSaveChanges();
            base.SaveChanges();
            OnAfterSaveChanges(auditEntries);
            return base.SaveChanges();
        }
        private List<AuditLog> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditLog>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BankAccount && (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
                {
                    var auditEntry = new AuditLog
                    {
                        TableName = entry.Metadata.GetTableName(),
                        Action = entry.State.ToString(),
                        NewData = ToJson(entry),
                        Timestamp = DateTime.UtcNow
                    };
                    auditEntries.Add(auditEntry);
                }
            }
            return auditEntries;
        }

        private void OnAfterSaveChanges(List<AuditLog> auditEntries)
        {
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry);
            }
            SaveChanges();
        }

        private string ToJson(EntityEntry entry)
        {
            var values = entry.CurrentValues;
            var jsonObject = new JObject();
            foreach (var property in values.Properties)
            {
                jsonObject.Add(property.Name, JToken.FromObject(values[property]));
            }
            return jsonObject.ToString(Formatting.None);
        }
        public DbSet<AccountHolder> AccountHolders { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountHolder>()
                .HasData(
            new AccountHolder { Id = 1, FirstName = "Thima", LastName = "Sigauque", DateOfBirth = new DateTime(1990, 1, 1) }
            );

            // Seed bank accounts
            modelBuilder.Entity<BankAccount>().HasData(
                new BankAccount { Id = 1, AccountNumber = "55555", AccountType = "Cheque", Name = "My Account", Status = "Active", AvailableBalance = 15000, AccountHolderId = 1 }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
