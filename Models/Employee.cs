using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the Employees Table in the BangazonAPI DB
    */
    public class Employee
    {

        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsSupervisor { get; set; }

        public int DepartmentId { get; set; }

    }
}
