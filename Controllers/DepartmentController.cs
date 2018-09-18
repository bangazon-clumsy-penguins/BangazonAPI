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

// Author: Evan Lusky
// Exposes Departments to users, allows user to add employees to department query.

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentsController(IConfiguration config)
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

        // GET: api/Departments/5
        // id comes from url ---^
        // Returns a single Department object
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM Departments WHERE Id = {id}";

                var theSingleDepartment = (await conn.QueryAsync<Department>(sql)).Single();
                return Ok(theSingleDepartment);
            }
        }


        //   GET /Departments
        //   GET /Departments?_include=employees
        //   _include comes from user defined value of _include in the query
        //   If _include = employees then returns a list of departments each with a list of their employees
        //   Else just returns a list of departments
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string _filter, string _gt)
        {
            string sql = "Select * FROM Departments AS A";

            if (_include != null && _include.Contains("employees"))
            {
                sql = "SELECT * FROM Departments AS A INNER JOIN Employees AS B ON A.Id = B.DepartmentId;";

            }

            if (_filter != null && _filter.Contains("budget"))
            {
                sql += $" WHERE A.budget > {_gt};";

            }

            using (IDbConnection conn = Connection)
            {
                if (_include == "employees")
                {
                    Dictionary<int, Department> departmentEmployees = new Dictionary<int, Department>();

                    var departmentsQuery = await conn.QueryAsync<Department, Employee, Department>(
                        sql,
                        (department, employee) =>
                        {
                            Department departmentEntry;

                            if (!departmentEmployees.TryGetValue(department.Id, out departmentEntry))
                            {
                                departmentEntry = department;
                                departmentEntry.EmployeeList = new List<Employee>();
                                departmentEmployees.Add(departmentEntry.Id, departmentEntry);
                            }

                            departmentEntry.EmployeeList.Add(employee);
                            return departmentEntry;
                        }); 

                    return Ok(departmentsQuery.Distinct()); //.values?
                    
                    

                }
                Console.WriteLine($"SQL: {sql}");
                var departments = await conn.QueryAsync<Department>(sql);
                return Ok(departments);

            }
        }


        // POST: api/Departments
        // department comes from body of JSON
        // Returns the object that was created
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            string sql = $@"INSERT INTO Departments
            (Name, Budget)
            VALUES
            ('{department.Name}', '{department.Budget}');
            select MAX(Id) from Departments";

            using (IDbConnection conn = Connection)
            {
                var newDepartmentId = (await conn.QueryAsync<int>(sql)).Single();
                department.Id = newDepartmentId;
                return CreatedAtRoute("GetDepartment", new { id = newDepartmentId }, department);
            }

        }


        // Checks to see if a department exists
        // id should be the id of the department 
        private bool DepartmentExists(int id)
        {
            string sql = $"SELECT Id, Name, Language FROM Departments WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Customer>(sql).Count() > 0;
            }
        }

        // PUT api/values/5
        // id comes from url of api call and determines which department will be replaced
        // department comes from the body of the call

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Department department)
        {
            string sql = $@"
            UPDATE Departments
            SET Name = '{department.Name}',
                Budget = '{department.Budget}'
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
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }




    }
}
