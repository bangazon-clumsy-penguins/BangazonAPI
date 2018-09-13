using System;
using System.Collections;
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
	[Route("/[controller]")]
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

		// GET: api/Trainings?completed=false
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string completed)
		{
			string sql = "SELECT * FROM Trainings";

			if (completed == "false")
			{
				DateTime today = DateTime.Today;
				sql += $" WHERE Trainings.EndDate >= '{today}'";
			}

			using (IDbConnection conn = Connection)
			{
				var allTrainings = await conn.QueryAsync<Training>(sql);
				return Ok(allTrainings);
			}
		}

		// GET: api/Trainings/5
		[HttpGet("{id}", Name = "GetSingleTraining")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			string sql = $"SELECT * FROM Trainings WHERE Id = {id}";
			using (IDbConnection conn = Connection)
			{
				Training singleTraining = (await conn.QueryAsync<Training>(sql)).Single();
				return Ok(singleTraining);
			}
		}

		// POST: api/Trainings
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Training training)
		{
			string sql = $@"
			INSERT INTO Trainings (Name, StartDate, EndDate, MaxOccupancy)
			VALUES ('{training.Name}', '{training.StartDate}', '{training.EndDate}', {training.MaxOccupancy});
            SELECT MAX(Id) from Trainings;
			";

			using (IDbConnection conn = Connection)
			{
				var trainingId = (await conn.QueryAsync<int>(sql)).Single();
				training.Id = trainingId;
				return CreatedAtRoute("GetSingleTraining", new { id = trainingId }, training);
			}
		}

		// PUT: api/Trainings/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Training training)
		{
			string sql = $@"
			UPDATE Trainings
			SET Name = '{training.Name}',
				StartDate = '{training.StartDate}',
				EndDate = '{training.EndDate}',
				MaxOccupancy = {training.MaxOccupancy}
			WHERE Id = {id};
			";

			try
			{
				using (IDbConnection conn = Connection)
				{
					int rowsAffected = await conn.ExecuteAsync(sql);
					if (rowsAffected > 0)
					{
						return new StatusCodeResult(StatusCodes.Status204NoContent);
					}
					throw new Exception("No rows affected");
				}
			}
			catch (Exception)
			{
				if (!TrainingExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			if (TrainingHasStarted(id))
			{
				throw new Exception("Cannot delete a training that has already started");
			}
			string sql = $@"DELETE FROM Trainings WHERE Id = {id}";

			using (IDbConnection conn = Connection)
			{
				int rowsAffected = await conn.ExecuteAsync(sql);
				if (rowsAffected > 0)
				{
					return new StatusCodeResult(StatusCodes.Status204NoContent);
				}
				throw new Exception("No rows affected");
			}
		}

		private bool TrainingExists(int id)
		{
			string sql = $"SELECT * FROM Trainings WHERE Id = {id}";
			using (IDbConnection conn = Connection)
			{
				return conn.Query<Training>(sql).Count() > 0;
			}
		}

		private bool TrainingHasStarted(int id)
		{
			string sql = $"SELECT * FROM Trainings WHERE Id = {id}";
			using (IDbConnection conn = Connection)
			{
				DateTime startDate = (conn.Query<Training>(sql)).Single().StartDate;
				DateTime today = DateTime.Today;
				return DateTime.Compare(startDate, today) < 0;
			}
		}

	}
}
