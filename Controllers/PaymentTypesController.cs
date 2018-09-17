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


/*
 AUTHORED BY: ADAM WIECKERT
 
 Purpose: To allow developers access to the PaymentTypes table in the BangazonAPI DB. Developers should be able to,
 GET all of the payment tpyes
 GET one payment type
 POST (Create) a payment type in the PaymentTypes table
 PUT (Update) a payment type in the PaymentTypes table

 Deletion of a payment type is not allowed
*/

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
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

        // GET: /PaymentType
        // Will return all PaymentTypes in an array
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

        // GET: /PaymentType/5
        // Will return an individual payment type as an object
        // The parameter of "id" received from the end of the route
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get(int id)
        {
            string sql = "";

            if (id != 0)
            {
                sql = $@"SELECT p.Id,
                                p.Label
                           FROM PaymentTypes p
                           WHERE p.Id = {id}";
            }
            else
            {
                sql = $"SELECT * FROM PaymentTypes";
            }

            using (IDbConnection conn = Connection)
            {
                try
                {
                    var paymentTypeQuery = (await conn.QueryAsync<PaymentType>(sql)).Single();
                    return Ok(paymentTypeQuery);
                }
                catch (InvalidOperationException)
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
            
            
        }

        // POST: /PaymentType
        // Creates a new payment type in the paymentTypes table
        // the parameter paymentType is of Class PaymentType and must be supplied in the body of the POST request
        // Label as a string is the only required property in the PaymentType
        // On successful POST the new Payment Type is returned as well as the route to the new Payment Type
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentType paymentType)
        {
            string sql = $@"INSERT INTO PaymentTypes (Label)
            VALUES ('{paymentType.Label}');
            SELECT MAX(Id) FROM PaymentTypes";

            using (IDbConnection conn = Connection)
            {
                var createPaymentType = (await conn.QueryAsync<int>(sql)).Single();
                paymentType.Id = createPaymentType;
                return CreatedAtRoute("GetPaymentType", new { id = createPaymentType }, paymentType);
            }

        }

        // PUT: /PaymentType/5
        // Parameter of "id" is supplied at the end of the route and "paymentType" within the body of the PUT request
        // paymentType requires a Label property
        // On successful update nothing is returned
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PaymentType paymentType)
        {
            string sql = $@"
            UPDATE PaymentTypes
            SET Label = '{paymentType.Label}'
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
            // If an item fails to update and an exception is thrown, if the item does exist, throw the exception, 
            // if item doesn't exist return not found.
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
