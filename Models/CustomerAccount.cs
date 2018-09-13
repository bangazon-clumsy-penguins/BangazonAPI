using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class CustomerAccount
    {
        [Key]
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
    }
}
