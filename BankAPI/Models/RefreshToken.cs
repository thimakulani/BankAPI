using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        [ForeignKey(nameof(Models.AccountHolder))]
        public int AccountHolderId { get; set; }
        public virtual AccountHolder AccountHolder { get; set; }
    }
}
