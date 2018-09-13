using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class EmployeeComputer
    {

        [Key]
        public int Id { get; set; }


        public DateTime AssignmentDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int EmployeeId { get; set; }

        public int ComputerId { get; set; }

        public Computer Computer { get; set; }

    }
}
