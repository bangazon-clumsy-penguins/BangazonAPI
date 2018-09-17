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

/*AUTHOR: Seth Dana
 * 
 * This is the Customers controller for the Bangazon public API. This controller has methods for get, create, and update. 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */

namespace BangazonAPI.Models
{
    //Set the route and the private _config variable to connect to the database

    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
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
            GET /Customers/active=false Queries for customers who don't have an active order (This param overrides all others except q)
            GET /Customers?q={FirstName} || {LastName} String query of customers first or last name. 
            GET /Customers?_include=payments Includes the customers payment options
            GET /Customers?_include=products Includes the customers products
            GET /Customers?_include=products,payments Includes both the payment options and products
         */
        [HttpGet]
        public async Task<IActionResult> Get(string q, string _include, string active)
        {
            string sql = "SELECT * FROM Customers";
            if (_include == null) { _include = ""; }

            if (_include.Contains("products"))
            {
                sql += " JOIN Products ON Products.CustomerId = Customers.Id";
            }

            if (_include.Contains("payments"))
            {
                sql += $@" JOIN Orders ON Customers.Id = Orders.CustomerId
                        JOIN CustomerAccounts ON Orders.CustomerAccountId = CustomerAccounts.Id
                        JOIN PaymentTypes ON PaymentTypes.Id = CustomerAccounts.PaymentTypeId";
            }
            if (active != null && active == "false")
            {
                sql = $@"SELECT * FROM Customers LEFT JOIN Orders ON Orders.CustomerId = Customers.Id
                        WHERE Orders.CustomerAccountId IS NULL";
            }


            if ((q != null || _include != null) && active != "false")
            {
                sql += " WHERE 1=1";
            }

            if (q != null)
            {
                sql += ($" AND FirstName LIKE '%{q}%' OR LastName LIKE '%{q}%'");
            }

            Console.WriteLine(sql);

            using (IDbConnection conn = Connection)
            {
                if (active != null && active == "false")
                {
                    var customersQuery = await conn.QueryAsync<Customer>(sql);
                    return Ok(customersQuery);
                }

                if (_include.Contains("payments") && _include.Contains("products"))
                {
                    Dictionary<int, Customer> customerData = new Dictionary<int, Customer>();
                    var customersQuery = await conn.QueryAsync<Customer, Product, Order, CustomerAccount, PaymentType, Customer>(
                        sql, (customer, product, order, customerAccount, paymentType) => {

                            Customer thisCustomer;
                            CustomerAccount thisCustomerAccount = customerAccount;
                            thisCustomerAccount.PaymentTypeName = $"{paymentType.Label}";

                            if (!customerData.TryGetValue(customer.Id, out thisCustomer))
                            {
                                thisCustomer = customer;
                                thisCustomer.CustomerAccountsList = new List<CustomerAccount>();
                                thisCustomer.CustomerProductsList = new List<Product>();
                                customerData.Add(thisCustomer.Id, thisCustomer);
                            }
                            thisCustomer.CustomerAccountsList.Add(customerAccount);
                            thisCustomer.CustomerProductsList.Add(product);
                            return thisCustomer;
                        });
                    return Ok(customersQuery.Distinct());
                }

                if (_include.Contains("payments"))
                {
                    Dictionary<int, Customer> customerPayments = new Dictionary<int, Customer>();

                    var customersQuery = await conn.QueryAsync<Customer, Order, CustomerAccount, PaymentType, Customer>(
                        sql,
                        (customer, order, customerAccount, paymentType) =>
                        {
                            Customer thisCustomer;
                            CustomerAccount thisCustomerAccount = customerAccount;

                            thisCustomerAccount.PaymentTypeName = $"{paymentType.Label}";
                            Console.WriteLine($"PaymentType: {paymentType.Label}");
                            if (!customerPayments.TryGetValue(customer.Id, out thisCustomer))
                            {
                                thisCustomer = customer;
                                thisCustomer.CustomerAccountsList = new List<CustomerAccount>();
                                customerPayments.Add(thisCustomer.Id, thisCustomer);
                            }

                            thisCustomer.CustomerAccountsList.Add(customerAccount);
                            return thisCustomer;
                        }
                    );
                    return Ok(customersQuery.Distinct());  // Used to be .Values

                }

                if (_include.Contains("products"))
                {
                    Dictionary<int, Customer> customerProducts = new Dictionary<int, Customer>();

                    var customersQuery = await conn.QueryAsync<Customer, Product, Customer>(sql, (customer, product) => 
                    {
                        Customer thisCustomer;
                        if (!customerProducts.TryGetValue(customer.Id, out thisCustomer))
                        {
                            thisCustomer = customer;
                            thisCustomer.CustomerProductsList = new List<Product>();
                            customerProducts.Add(thisCustomer.Id, thisCustomer);
                        }

                        thisCustomer.CustomerProductsList.Add(product);

                        return thisCustomer;
                    });
                    return Ok(customersQuery.Distinct());
                }

                if (q != null)
                {
                    var customersQuery = await conn.QueryAsync<Customer>(sql);
                    Ok(customersQuery);
                }

                IEnumerable<Customer> customers = await conn.QueryAsync<Customer>(sql);
                return Ok(customers);
            }
        }

        // GET /Customers/5
        //Get a single customer
        //Customer Id must be passed from the url
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            string sql = $"SELECT * FROM Customers WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                try
                {
                Customer customers = (await conn.QueryAsync<Customer>(sql)).Single();
                return Ok(customers);
                }

                catch (InvalidOperationException)
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        //POST /customers
        //Post a customer object
        //Must match Customer model. FirstName, LastName, JoinDate, and LastInteractionDate must be passed.
       [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            var thisDate = DateTime.Now;
            customer.JoinDate = thisDate;
            customer.LastInteractionDate = thisDate;
            string sql = $@"INSERT INTO Customers
            (FirstName, LastName, JoinDate, LastInteractionDate)
            VALUES
            ('{customer.FirstName}', '{customer.LastName}', '{customer.JoinDate}', '{customer.LastInteractionDate}');
            select MAX(Id) from Customers;";

            Console.WriteLine(sql);
            using (IDbConnection conn = Connection)
            {
                var customerId = (await conn.QueryAsync<int>(sql)).Single();
                customer.Id = customerId;
                return CreatedAtRoute("GetCustomer", new { id = customerId }, customer);
            }
        }

        /*
            PUT /customers/5
            Must match Customer model. FirstName, LastName, and LastInteractionDate are required params.
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeCustomer(int id, [FromBody] Customer customer)
        {
            customer.LastInteractionDate = DateTime.Now;
            string sql = $@"
            UPDATE Customers
            SET FirstName = '{customer.FirstName}', LastName = '{customer.LastName}', LastInteractionDate = '{customer.LastInteractionDate}'
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /*Leaving this code in for the future possibility of wanting to delete a customer.
         * 
         * 
         * 
         **/

        //DELETE /customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    string sql = $@"DELETE FROM Customers WHERE Id = {id}";

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

        //This method is run when an exception is thrown because nothing has been altered in the database. It checks if a customer exists.
        private bool CustomerExists(int id)
        {
            string sql = $"SELECT Id, FirstName, LastName FROM Customers WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Customer>(sql).Count() > 0;
            }
        }
    }
}
