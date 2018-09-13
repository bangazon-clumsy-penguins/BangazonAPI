using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TrainingsController : ControllerBase
	{

		private readonly IConfiguration _config;

		public TrainingsController(IConfiguration config)
		{
			_config = config;
		}

		public IDbConnection Connection
		{
			get
			{
				return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			}
		}

		// GET: api/Trainings
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			using (IDbConnection conn = Connection)
			{
				string sql = "SELECT * FROM Trainings";

				var fullExercises = await conn.QueryAsync<Training>(sql);
				return Ok(fullExercises);
			}
		}

		// GET: api/Trainings/5
		[HttpGet("{id}", Name = "Get")]
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/Trainings
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT: api/Trainings/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
