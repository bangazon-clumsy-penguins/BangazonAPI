using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the CustomerAccounts Table in the BangazonAPI DB
    */
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
