using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string NewData { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
