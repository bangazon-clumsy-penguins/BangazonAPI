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

namespace BangazonAPI.Models
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
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

        //    GET /Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = "SELECT * FROM Products ";

            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            { 
                IEnumerable<Product> products = await conn.QueryAsync<Product>(sql);
                return Ok(products);
            }
        }

        // GET /Products/5
        [HttpGet("{id}", Name = "GetProducts")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            string sql = $"SELECT * FROM Products WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Product> product = await conn.QueryAsync<Product>(sql);
                return Ok(product);
            }
        }

        //POST /Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            string sql = $@"INSERT INTO Products
            (Title, Description, Quantity, Price, ProductTypeId, CustomerId)
            VALUES
            ('{product.Title}', '{product.Description}', '{product.Quantity}', '{product.Price}', '{product.ProductTypeId}', '{product.CustomerId}');
            select MAX(Id) from Products;";

            Console.WriteLine(sql);
            using (IDbConnection conn = Connection)
            {
                var productId = (await conn.QueryAsync<int>(sql)).Single();
                product.Id = productId;
                return CreatedAtRoute("GetProducts", new { id = productId }, product);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeProduct(int id, [FromBody] Product product)
        {
            string sql = $@"
            UPDATE Products
            SET Title = '{product.Title}', Description = '{product.Description}', Quantity = '{product.Quantity}', Price = '{product.Price}', ProductTypeId = '{product.ProductTypeId}', CustomerId = '{product.CustomerId}'
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

       // DELETE /Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = $@"DELETE FROM Products WHERE Id = {id}";

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
            string sql = $"SELECT Id FROM Products WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Product>(sql).Count() > 0;
            }
        }
    }
}