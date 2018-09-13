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

        // GET: api/Departments
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    string sql = "SELECT * FROM Departments";

        //    using (IDbConnection conn = Connection)
        //    {
        //        var departments = await conn.QueryAsync<Department>(sql);
        //        return Ok(departments);
        //    }
        //}

        // GET: api/Departments/5
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



        //   GET /Departments?_include=employees
        [HttpGet]
        public async Task<IActionResult> Get(string _include)
        {
            string sql = "Select * FROM Departments";

            if (_include != null && _include.Contains("employees"))
            {
                sql = "SELECT * FROM Departments AS A INNER JOIN Employees AS B ON A.Id = B.DepartmentId;";

            }

            using (IDbConnection conn = Connection)
            {
                if (_include == "employees")
                {
                    Dictionary<int, Department> departmentEmployees = new Dictionary<int, Department>();

                    var customersQuery = await conn.QueryAsync<Department, Employee, Department>(
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

                    return Ok(customersQuery.Distinct()); //.values?
                    
                    

                }
                var departments = await conn.QueryAsync<Department>(sql);
                return Ok(departments);

            }
        }


        // POST: api/Departments
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

        private bool DepartmentExists(int id)
        {
            string sql = $"SELECT Id, Name, Language FROM Departments WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Customer>(sql).Count() > 0;
            }
        }

        // PUT api/values/5
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
