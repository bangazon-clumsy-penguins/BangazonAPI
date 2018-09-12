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
    public class ProductTypeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductTypeController(IConfiguration config)
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
            GET /ProductTypes
            GET /ProductTypes?=label
         */

        [HttpGet]
        public async Task<IActionResult> Get(string label)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM ProductTypes";

                if (label != null)
                {
                    sql += $" WHERE Label='{label}'";
                }

                var productTypes = await conn.QueryAsync<ProductType>(sql);
                return Ok(productTypes);
            }

        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProductType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM ProductType WHERE Id = {id}";

                var singleProductType = (await conn.QueryAsync<ProductType>(sql)).Single();
                return Ok(singleProductType);
            }
        }

        // POST /ProductTypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductType ProductType)
        {
            string sql = $@"INSERT INTO ProductType
            (Label)
            VALUES
            ('{ProductType.Label}');
            select MAX(Id) from ProductType;";

            using (IDbConnection conn = Connection)
            {
                var ProductTypeId = (await conn.QueryAsync<int>(sql)).Single();
                ProductType.Id = ProductTypeId;
                return CreatedAtRoute("GetProductType", new { id = ProductTypeId }, ProductType);
            }
        }

        /*
            PUT /ProductTypes/5

            The [HttpPut] attribute ensures that this method will handle any
            request to a `/ProductTypes/{id}` with the PUT HTTP verb. Alternatively,
            I could name this method `PutProductType`, or just `Put` and ASP.NET
            will detect that the word `Put` is in the method name and ensure
            that it will only be invoke for PUT operations.

            All other controllers have this method named as `Put`. It's named
            differently here to show that the [HttpPut] attribute enforces which
            verb is handled.
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeProductType(int id, [FromBody] ProductType ProductType)
        {
            string sql = $@"
            UPDATE ProductType
            SET Label = '{ProductType.Label}'
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
                if (!ProductTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE /ProductTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = $@"DELETE FROM ProductType WHERE Id = {id}";

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

        private bool ProductTypeExists(int id)
        {
            string sql = $"SELECT Id, Label FROM ProductType WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<ProductType>(sql).Count() > 0;
            }
        }
    }
}
