using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the Products Table in the BangazonAPI DB
    */
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public int ProductTypeId { get; set; }

        public int CustomerId { get; set; }
    }
}
