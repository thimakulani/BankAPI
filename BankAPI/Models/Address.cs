using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        [ForeignKey(nameof(Models.AccountHolder))]
        public int AccountHolderId { get; set; }
        public virtual AccountHolder AccountHolder { get; set; } 
    }
}
