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
