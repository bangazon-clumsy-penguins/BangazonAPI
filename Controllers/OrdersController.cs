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
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrdersController(IConfiguration config)
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

        // GET: /Orders
        // GET: /Orders?completed=false
        [HttpGet]
        public async Task<IActionResult> Get(string completed)
        {
            string sql = "SELECT * FROM Orders";

            if (completed != null && completed == "false")
            {
                sql += " WHERE CustomerAccountId IS null;";
            }

            using (IDbConnection conn = Connection)
            {
                var orders = await conn.QueryAsync<Order>(sql);
                return Ok(orders);
            }
        }

        // GET: /Orders/5
        [HttpGet("{id}", Name = "GetSingleOrder")]
        public async Task<IActionResult> Get(int id)
        {
            string sql = $"SELECT * FROM Orders o WHERE o.Id = {id}";


            //SELECT*
            //FROM Orders o
            //JOIN OrderedProducts op on o.Id = op.OrderId
            //JOIN Products p on op.ProductId = p.Id
            //WHERE o.Id = 3;

            using (IDbConnection conn = Connection)
            {
                var singleOrder = (await conn.QueryAsync<Order>(sql)).Single();
                return Ok(singleOrder);
            }
        }

        // POST: /Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            string sql = $@"INSERT INTO Orders
            (CustomerId, CustomerAccountId)
            VALUES
            ('{order.CustomerId}', '{order.CustomerAccountId}');
            select MAX(Id) from Orders;";

            using (IDbConnection conn = Connection)
            {
                var createdOrder = (await conn.QueryAsync<int>(sql)).Single();
                order.Id = createdOrder;
                return CreatedAtRoute("GetSingleOrder", new { id = createdOrder }, order);
            }
        }

        // PUT: /Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            string sql = $@"
            UPDATE Orders
            SET 
                CustomerId = {order.CustomerId}, 
                CustomerAccountId = {order.CustomerAccountId}
            WHERE Id = {id};";

            using (IDbConnection conn = Connection)
            {
                int rows = await conn.ExecuteAsync(sql);
                if (rows > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                throw new Exception("No rows affected");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"
                DELETE op
                FROM OrderedProducts op
                WHERE op.OrderId = {id};

                DELETE o
                FROM Orders o
                WHERE o.Id = {id};";

            using (IDbConnection conn = Connection)
            {
                int deleteOrdersAndOrderedProducts = await conn.ExecuteAsync(sql);
                if (deleteOrdersAndOrderedProducts > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                throw new Exception("No rows affected");
            }
        }

        private bool OrderExists(int id)
        {
            string sql = $"SELECT * FROM Orders WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Order>(sql).Count() > 0;
            }
        }
    }
}
