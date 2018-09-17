using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

//This class is representative of the Customers table in Bangazon.db

namespace BangazonAPI.Models
{
    public class Customer
    {
        /* 
         AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

         PURPOSE: Model to reflect the items on the Customers Table in the BangazonAPI DB
        */
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastInteractionDate { get; set; }

        public List<Product> CustomerProductsList { get; set; }
        //public List<Order> CustomerOrdersList { get; set; }
        public List<CustomerAccount> CustomerAccountsList { get; set; }

    }
}
