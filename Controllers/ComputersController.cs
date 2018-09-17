using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
// Author: Evan Lusky
// Exposes Computers to users.

namespace BangazonAPI.Models
{
    [Route("[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ComputersController(IConfiguration config)
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

        //    GET /Computers
        // Returns full list of Computers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = "SELECT * FROM Computers ";

            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Computer> computers = await conn.QueryAsync<Computer>(sql);
                return Ok(computers);
            }
        }

        // GET /Computers/5
        // id comes from url
        // Returns a single Computer object
        [HttpGet("{id}", Name = "GetComputers")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            string sql = $"SELECT * FROM Computers WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Computer> computer = await conn.QueryAsync<Computer>(sql);
                return Ok(computer);
            }
        }

        //POST /Computers
        // computer comes from body of JSON
        // Returns the object that was created
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Computer computer)
        {
            string sql = $@"INSERT INTO Computers
            (Model, PurchaseDate, DecommissionDate)
            VALUES
            ('{computer.Model}', '{computer.PurchaseDate}', '{computer.DecommissionDate}');
            select MAX(Id) from Computers;";

            Console.WriteLine(sql);
            using (IDbConnection conn = Connection)
            {
                var computerId = (await conn.QueryAsync<int>(sql)).Single();
                computer.Id = computerId;
                return CreatedAtRoute("GetComputers", new { id = computerId }, computer);
            }
        }

        // PUT api/values/5
        // id comes from url of api call and determines which department will be replaced
        // department comes from the body of the call
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeProduct(int id, [FromBody] Computer computer)
        {
            string sql = $@"
            UPDATE Computers
            SET Model = '{computer.Model}', PurchaseDate = '{computer.PurchaseDate}', DecommissionDate = '{computer.DecommissionDate}' 
            WHERE Id = {id}";

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
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE /Computers/5
        // id comes from url
        // Deletes Computer 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = $@"DELETE FROM Computers WHERE Id = {id}";

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

        private bool ProductExists(int id)
        {
            string sql = $"SELECT Id FROM Computers WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Computer>(sql).Count() > 0;
            }
        }
    }
}
