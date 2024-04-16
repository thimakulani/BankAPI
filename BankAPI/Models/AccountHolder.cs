using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BankAPI.Models
{
    public class AccountHolder
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; internal set; }
        public string IdNumber { get; internal set; }
        public string Email { get; set; } 
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Address Address { get; set; }
    }
}
