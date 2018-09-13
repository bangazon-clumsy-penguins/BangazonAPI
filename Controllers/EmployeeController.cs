﻿using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

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
            GET /Employees?q=test
            GET /Employees?_include=payments
         */
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = $@"
                            SELECT *
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
                                                            EmployeeComputer,
                                                            Computer,
                                                            Employee>(
                    sql,
                    (Employee, Department, EmployeeComputer, Computer) =>
                    {
                        Employee.Department = Department;
                        Employee.EmployeeComputer = EmployeeComputer;
                        Employee.EmployeeComputer.Computer = Computer;

                        return Employee;

                    }
                );
                
                return Ok(EmployeesQuery);  // Used to be .Values 
            }
        }

        // GET /Employees/5
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
      
            string sql = $@"
                            SELECT *
                            FROM Employees e
                            LEFT OUTER JOIN Departments d on d.Id = e.DepartmentId
                            LEFT OUTER JOIN EmployeeComputers ec on ec.EmployeeId = e.Id 
	                            LEFT OUTER JOIN Computers c on c.Id = ec.ComputerId
                            WHERE ec.ReturnDate is null
                                AND e.Id = {id}
                            ";

            using (IDbConnection conn = Connection)
            {


                var EmployeesQuery = await conn.QueryAsync<Employee,
                                                            Department,
                                                            EmployeeComputer,
                                                            Computer,
                                                            Employee>(
                    sql,
                    (Employee, Department, EmployeeComputer, Computer) =>
                    {
                        Employee.Department = Department;
                        Employee.EmployeeComputer = EmployeeComputer;
                        Employee.EmployeeComputer.Computer = Computer;

                        return Employee;

                    }
                );

                return Ok(EmployeesQuery.Single());  // Used to be .Values 
            }
        }

        //// POST /Employees
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Employee Employee)
        //{
        //    string sql = $@"INSERT INTO Employee
        //    ()
        //    VALUES
        //    ();
        //    select MAX(Id) from Employee;";

        //    using (IDbConnection conn = Connection)
        //    {
        //        var EmployeeId = (await conn.QueryAsync<int>(sql)).Single();
        //        Employee.EmployeeId = EmployeeId;
        //        return CreatedAtRoute("GetEmployee", new { id = EmployeeId }, Employee);
        //    }
        //}

        ///*
        //    PUT /Employees/5

        //    The [HttpPut] attribute ensures that this method will handle any
        //    request to a `/Employees/{id}` with the PUT HTTP verb. Alternatively,
        //    I could name this method `PutEmployee`, or just `Put` and ASP.NET
        //    will detect that the word `Put` is in the method name and ensure
        //    that it will only be invoke for PUT operations.

        //    All other controllers have this method named as `Put`. It's named
        //    differently here to show that the [HttpPut] attribute enforces which
        //    verb is handled.
        // */
        //[HttpPut("{id}")]
        //public async Task<IActionResult> ChangeEmployee(int id, [FromBody] Employee Employee)
        //{
        //    string sql = $@"
        //    UPDATE Employee
        //    SET '
        //    WHERE Id = {id}";

        //    try
        //    {
        //        using (IDbConnection conn = Connection)
        //        {
        //            int rowsAffected = await conn.ExecuteAsync(sql);
        //            if (rowsAffected > 0)
        //            {
        //                return new StatusCodeResult(StatusCodes.Status204NoContent);
        //            }
        //            throw new Exception("No rows affected");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!EmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //// DELETE /Employees/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    string sql = $@"DELETE FROM Employee WHERE Id = {id}";

        //    using (IDbConnection conn = Connection)
        //    {
        //        int rowsAffected = await conn.ExecuteAsync(sql);
        //        if (rowsAffected > 0)
        //        {
        //            return new StatusCodeResult(StatusCodes.Status204NoContent);
        //        }
        //        throw new Exception("No rows affected");
        //    }

        //}

        //private bool EmployeeExists(int id)
        //{
        //    string sql = $"SELECT Id, Name, Language FROM Employee WHERE Id = {id}";
        //    using (IDbConnection conn = Connection)
        //    {
        //        return conn.Query<Employee>(sql).Count() > 0;
        //    }
        //}
    }
}
