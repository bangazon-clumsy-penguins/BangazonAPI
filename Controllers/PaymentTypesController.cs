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
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypesController : ControllerBase
    {

        private readonly IConfiguration _config;

        public PaymentTypesController(IConfiguration config)
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

        // GET: api/PaymentType
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = "SELECT * FROM PaymentTypes";

            using (IDbConnection conn = Connection)
            {
                var allPaymentTypesQuery = await conn.QueryAsync<PaymentType>(sql);
                return Ok(allPaymentTypesQuery);
            }
            
        }

        // GET: api/PaymentType/5
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get(int id)
        {
            string sql = "";

            if (id != 0)
            {
                sql = $@"SELECT p.PaymentTypeId,
                                   p.Type,
                           FROM PaymentTypes p
                           WHERE {id} = p.PaymentTypeId";
            }
            else
            {
                sql = $"SELECT * FROM PaymentTypes";
            }

            using (IDbConnection conn = Connection)
            {
                var paymentTypeQuery = await conn.QueryAsync<PaymentType>(sql);
                return Ok(paymentTypeQuery);
            }
            
            
        }

        // POST: api/PaymentType
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PaymentType paymentType)
        {
            string sql = $@"INSERT INTO PaymentTypes (Type)
            VALUES ({paymentType.Label});
            SELECT MAX(PaymentTypeId) FROM PaymentTypes";

            using (IDbConnection conn = Connection)
            {
                var createPaymentType = (await conn.QueryAsync<int>(sql)).Single();
                paymentType.PaymentTypeId = createPaymentType;
                return CreatedAtRoute("GetPaymentType", new { id = createPaymentType }, paymentType);
            }

        }

        // PUT: api/PaymentType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PaymentType paymentType)
        {
            string sql = $@"
            UPDATE PaymentTypes
            SET Label = {paymentType.Label}'
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
                    else
                    {
                        throw new Exception("No Rows Edited");
                    }
                }
            }
            catch (Exception)
            {
                if (!PaymentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;   
                }
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = $"DELETE FROM PaymentTypes p WHERE p.Id = {id}";

            using (IDbConnection conn = Connection)
            {
                var deletePaymentType = await conn.ExecuteAsync(sql);
                if (deletePaymentType > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                else
                {
                    throw new Exception("No PaymentType Found");
                }
            }
        }


        private bool PaymentTypeExists(int id)
        {
            string sql = $"SELECT Id, Label FROM PaymentTypes WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<PaymentType>(sql).Count() > 0;
            }
        }
    }
}
