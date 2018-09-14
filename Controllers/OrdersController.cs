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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = "SELECT * FROM Orders";

            using (IDbConnection conn = Connection)
            {
                var orders = await conn.QueryAsync<Order>(sql);
                return Ok(orders);
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}", Name = "GetSingleOrder")]
        public async Task<IActionResult> Get(int id)
        {
            string sql = $"SELECT * FROM Orders o WHERE o.Id = {id}";

            using (IDbConnection conn = Connection)
            {
                var singleOrder = (await conn.QueryAsync<Order>(sql)).Single();
                return Ok(singleOrder);
            }
        }

        // POST: api/Orders
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Orders/5
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
