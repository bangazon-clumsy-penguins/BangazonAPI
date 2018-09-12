using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Computer
    {
        [Key]
        public int Id { get; set; }

        public string Model { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PurchaseDate{ get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DecommisionDate { get; set; }

    }
}
