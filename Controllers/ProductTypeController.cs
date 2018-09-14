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
Purpose: Allow API client to interact with the Product Type resouce
            -retrieve all or single product type
            -add new product type
            -modify an existing product type
            -delete a product type

Author: Phil Patton
*/

namespace BangazonAPI.Models
{
    [Route("[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductTypesController(IConfiguration config)
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
            returns an array of all product types
         */

        [HttpGet]
        public async Task<IActionResult> Get()
        {
                
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM ProductTypes";

                var productTypes = await conn.QueryAsync<ProductType>(sql);
                return Ok(productTypes);
            }

        }

        // GET /ProductTypes/5
        // returns a single product type with Id 5

        [HttpGet("{id}", Name = "GetProductType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = $"SELECT * FROM ProductTypes WHERE Id = {id}";

                    var singleProductType = (await conn.QueryAsync<ProductType>(sql)).Single();
                    return Ok(singleProductType);
                }
            }

            catch(Exception e)
            {
                Console.WriteLine($@"ProductType Get ERROR:
                                    {e.Message}");
                return NotFound();
            }
            
        }

        // POST /ProductTypes
        // Adds new product type

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductType ProductType)
        {
            string sql = $@"INSERT INTO ProductTypes
            (Label)
            VALUES
            ('{ProductType.Label}');
            select MAX(Id) from ProductTypes;";

            using (IDbConnection conn = Connection)
            {
                var ProductTypeId = (await conn.QueryAsync<int>(sql)).Single();
                ProductType.Id = ProductTypeId;
                return CreatedAtRoute("GetProductType", new { id = ProductTypeId }, ProductType);
            }
        }

        // Update /ProductTypes/5
        // Update product type with Id 5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductType ProductType)
        {
            string sql = $@"
            UPDATE ProductTypes
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
            string sql = $@"DELETE FROM ProductTypes WHERE Id = {id}";

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
            string sql = $"SELECT Id, Label FROM ProductTypes WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<ProductType>(sql).Count() > 0;
            }
        }
    }
}
