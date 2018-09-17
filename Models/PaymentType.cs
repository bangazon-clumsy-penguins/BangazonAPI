using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the PaymentTypes Table in the BangazonAPI DB
    */
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
