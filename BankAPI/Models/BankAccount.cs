using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal AvailableBalance { get; set; }
        [ForeignKey(nameof(Models.AccountHolder))]
        public int AccountHolderId { get; set; }
        public virtual AccountHolder AccountHolder { get; set; }
    }
}
