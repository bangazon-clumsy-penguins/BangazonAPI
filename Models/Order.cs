using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BangazonAPI.Models
{
  public class Order
  {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int? CustomerAccountId { get; set; }


  }
}