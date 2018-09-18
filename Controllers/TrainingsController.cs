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

		// GET: /Trainings
		// GET: /Trainings?completed=false
		// Purpose: GET a single Training object from the Trainings table in the database
		// Arguments: completed=false can be passed in the query string to show only trainings that have not ended
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string completed)
		{
			string sql = @"
			SELECT 
				t.Id, t.Name, t.StartDate, t.EndDate, t.MaxOccupancy,
				e.Id, e.FirstName, e.LastName, e.HireDate, e.IsSupervisor, e.DepartmentId
				FROM Trainings t
				JOIN EmployeeTrainings et ON t.Id = et.TrainingId
				JOIN Employees e ON et.EmployeeId = e.Id
			";

			if (completed == "false")
			{
				DateTime today = DateTime.Today;
				sql += $" WHERE Trainings.EndDate >= '{today}'";
			}

			using (IDbConnection conn = Connection)
			{
				Dictionary<int, Training> trainingDictionary = new Dictionary<int, Training>();
				var apiResponse = await conn.QueryAsync<Training, Employee, Training>(sql,
					(training, employee) =>
					{

						if (!(trainingDictionary.ContainsKey(training.Id)))
						{
							trainingDictionary[training.Id] = training;
						}
						trainingDictionary[training.Id].RegisteredEmployees.Add(employee);

						return trainingDictionary[training.Id];
					});

				List<Training> allTrainings = apiResponse.Distinct().ToList();

				return Ok(allTrainings);
			}
		}

		// GET: /Trainings/5
		// Purpose: GET a single Training object from the Trainings table in the database
		// Arguments: The Id of the training to be returned is passed in the route
		[HttpGet("{id}", Name = "GetSingleTraining")]
		public async Task<IActionResult> Get([FromRoute] int id)
		{
			string sql = $@"
			SELECT 
				t.Id, t.Name, t.StartDate, t.EndDate, t.MaxOccupancy,
				e.Id, e.FirstName, e.LastName, e.HireDate, e.IsSupervisor, e.DepartmentId
				FROM Trainings t
				JOIN EmployeeTrainings et ON t.Id = et.TrainingId
				JOIN Employees e ON et.EmployeeId = e.Id
			WHERE t.Id = {id}
			";

			using (IDbConnection conn = Connection)
			{
				Dictionary<int, Training> trainingDictionary = new Dictionary<int, Training>();

				var apiResponse = await conn.QueryAsync<Training, Employee, Training>(sql,
					(training, employee) =>
					{
						if (!(trainingDictionary.ContainsKey(training.Id)))
						{
							trainingDictionary[training.Id] = training;
						}
						trainingDictionary[training.Id].RegisteredEmployees.Add(employee);

						return trainingDictionary[training.Id];
					});

				try
				{
					Training singleTraining = apiResponse.Distinct().Single();
					return Ok(singleTraining);
				}
				catch (InvalidOperationException)
				{
					return new StatusCodeResult(StatusCodes.Status404NotFound);
				}

			}
		}

		/* 
		POST: /Trainings
		Purpose: POST a new training object to the Trainings table in the database
		Arguments: A JSON-formatted object must be passed in the body of the request
		The object must match the following format:
		
		{
			"Name": "Name of Training",
			"StartDate": "YYYY-MM-DDT00:00:00",
			"EndDate": "YYYY-MM-DDT00:00:00",
			"MaxOccupancy": 42
		}

		The MaxOccupancy property must be a positive integer and the EndDate property must not be before the StartDate.
		Otherwise, an exception will be thrown and the item will not be created
		*/
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Training training)
		{
			if (training.MaxOccupancy <= 0)
			{
				throw new Exception("The property 'MaxOccupancy' must be greater than 0; a training cannot have zero or negative occupants");
			}

			if(training.EndDate < training.StartDate)
			{
				throw new Exception("The property 'EndDate' must be greater than 'StartDate'; a training cannot end before it has started");
			}

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

		/* 
		PUT: /Trainings/5
		Purpose: Edit one or more properties of a training object in the Trainings table of the database
		Arguments:	The Id of the training to be edited 
					A JSON-formatted object must be passed in the body of the request
		The object must match the following format:

		{
			"Name": "Name of Training",
			"StartDate": "YYYY-MM-DDT00:00:00",
			"EndDate": "YYYY-MM-DDT00:00:00",
			"MaxOccupancy": 42
		}

		The MaxOccupancy property must be a positive integer and the EndDate property must not be before the StartDate.
		Otherwise, an exception will be thrown and the item will not be edited
		*/
		[HttpPut("{id}")]
		public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Training training)
		{
			if (training.MaxOccupancy <= 0)
			{
				throw new Exception("The property 'MaxOccupancy' must be greater than 0; a training cannot have zero or negative occupants");
			}

			if (training.EndDate < training.StartDate)
			{
				throw new Exception("The property 'EndDate' must be greater than 'StartDate'; a training cannot end before it has started");
			}

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

		/* 
		DELETE: /Trainings/5
		Purpose: Delete a training object from the Trainings table of the database
		Arguments:	The Id of the training to be deleted

		The "StartDate" of the training to be deleted must be in the future.
		Otherwise, an exception will be thrown and the item will not be deleted
		*/
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			if (TrainingHasStarted(id))
			{
				throw new Exception("The property 'StartDate' must be in the future; cannot delete a training that has already started");
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
