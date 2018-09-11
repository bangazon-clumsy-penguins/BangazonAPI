﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int ProductTypeId { get; set; }
        public int CustomerId { get; set; }
    }
}