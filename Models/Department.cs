using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    /* 
     AUTHORED: Adam Wieckert, Seth Dana, Elliot Huck, Evan Lusky, Phil Patton

     PURPOSE: Model to reflect the items on the Departments Table in the BangazonAPI DB
    */
    public class Department
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        public List<Employee> EmployeeList { get; set; }

    }
}
