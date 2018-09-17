using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

/*
Purpose: Allow API client to interact with the Employee resouce
            -retrieve all or single employee with department and computer information attached
            -add new employee to database
            -modify an employees information

Author: Phil Patton
*/

namespace BangazonAPI.Models
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
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

        /*
            GET /Employees
            Returns all employees with department and computer information if available
         */
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = $@"
                            SELECT e.*
                                ,d.*
                                ,c.*
                            FROM Employees e
                            LEFT OUTER JOIN Departments d on d.Id = e.DepartmentId
                            LEFT OUTER JOIN EmployeeComputers ec on ec.EmployeeId = e.Id 
	                            LEFT OUTER JOIN Computers c on c.Id = ec.ComputerId
                            WHERE ec.ReturnDate is null
                            ";

            using (IDbConnection conn = Connection)
            {


                var EmployeesQuery = await conn.QueryAsync<Employee,
                                                            Department,
                                                            Computer,
                                                            Employee>(
                    sql,
                    (Employee, Department, Computer) =>
                    {
                        Employee.Department = Department;
                        Employee.Computer = Computer;

                        return Employee;

                    }
                );
                
                return Ok(EmployeesQuery);  // Used to be .Values 
            }
        }

        // GET /Employees/5
        // Returns a single employee object based on Id in route 
        // with computer and department information attached
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
      
            string sql = $@"
                            SELECT e.*
                                ,d.*
                                ,c.*
                            FROM Employees e
                            LEFT OUTER JOIN Departments d on d.Id = e.DepartmentId
                            LEFT OUTER JOIN EmployeeComputers ec on ec.EmployeeId = e.Id 
	                            LEFT OUTER JOIN Computers c on c.Id = ec.ComputerId
                            WHERE ec.ReturnDate is null
                                AND e.Id = {id}
                            ";

            using (IDbConnection conn = Connection)
            {

                try
                {
                    var EmployeesQuery = await conn.QueryAsync<Employee,
                                                            Department,
                                                            Computer,
                                                            Employee>(
                        sql,
                        (Employee, Department, Computer) =>
                        {
                            Employee.Department = Department;
                            Employee.Computer = Computer;

                            return Employee;

                        }
                    );

                    return Ok(EmployeesQuery.Single());  // Used to be .Values 
                }

                catch(Exception e)
                {
                    Console.WriteLine($@"Employee Get Error Message:
                    {e.Message}");
                    return NotFound();
                }
                
            }
        }

        // POST /Employees
        // Adds new Employee to database
        // Parameters: FirstName, LastName, HireDate, isSupervisor, DepartmentId
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee Employee)
        {
            string sql = $@"
            INSERT INTO Employees (FirstName, LastName, HireDate, IsSupervisor, DepartmentId)
            VALUES ('{Employee.FirstName}'
                    ,'{Employee.LastName}'
                    ,'{Employee.HireDate}'
                    ,{Convert.ToInt32(Employee.IsSupervisor)}
                    ,{Employee.DepartmentId});
            select MAX(Id) from Employees;";

            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            {
                var EmployeeId = (await conn.QueryAsync<int>(sql)).Single();
                Employee.Id = EmployeeId;
                return CreatedAtRoute("GetEmployee", new { id = EmployeeId }, Employee);
            }
        }

        ///*
        //    PUT /Employees/5
        //      Updates Employee information for the employee with the ID in the route
        // Parameters: FirstName, LastName, HireDate, isSupervisor, DepartmentId
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeEmployee(int id, [FromBody] Employee Employee)
        {
            string sql = $@"
            UPDATE Employees
            SET FirstName = '{Employee.FirstName}'
				,LastName = '{Employee.LastName}'
				,HireDate = '{Employee.HireDate}'
				,IsSupervisor = {Convert.ToInt32(Employee.IsSupervisor)}
				,DepartmentId = {Employee.DepartmentId}
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
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Checks for the existence of Employee based on Id parameter passed and returns a boolean
        private bool EmployeeExists(int id)
        {
            string sql = $"SELECT Id, FirstName, LastName, HireDate, IsSupervisor, DepartmentId FROM Employees WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Employee>(sql).Count() > 0;
            }
        }
    }
}
