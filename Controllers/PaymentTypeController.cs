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
    public class PaymentTypeController : ControllerBase
    {

        private readonly IConfiguration _config;

        public PaymentTypeController(IConfiguration config) {
            _config = config;
        }

        public IDbConnection Connection {
            get {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/PaymentType
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = "SELECT * FROM PaymentType";

            using (IDbConnection conn = Connection) {
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
                           FROM PaymentType p
                           WHERE {id} = p.PaymentTypeId";
            }
            else {
                sql = $"SELECT * FROM PaymentType";
            }

            using (IDbConnection conn = Connection) {
                var paymentTypeQuery = await conn.QueryAsync<PaymentType>(sql);
                return Ok(paymentTypeQuery);
            }
            
            
        }

        // POST: api/PaymentType
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PaymentType paymentType)
        {
            string sql = $@"INSERT INTO PaymentType (Type)
            VALUES ({paymentType.Type});
            SELECT MAX(PaymentTypeId) FROM PaymentType";

            using (IDbConnection conn = Connection) {
                var createPaymentType = (await conn.QueryAsync<int>(sql)).Single();
                paymentType.PaymentTypeId = createPaymentType;
                return CreatedAtRoute("GetPaymentType", new { id = createPaymentType}, paymentType);
            }

        }

        // PUT: api/PaymentType/5
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
