using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
	public class Training
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int MaxOccupancy { get; set; }

		public List<Employee> RegisteredEmployees = new List<Employee>();
    }
}
