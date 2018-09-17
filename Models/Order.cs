using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the Orders Table in the BangazonAPI DB
    */
    public class Order
  {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int? CustomerAccountId { get; set; }

        public List<Product> ProductsOnOrder = new List<Product>();

        public Customer CustomerOnOrder { get; set; }


  }
}