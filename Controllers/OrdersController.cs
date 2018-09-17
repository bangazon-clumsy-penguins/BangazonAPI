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
 
 Purpose: To allow developers access to the Orders table in the BangazonAPI DB. Developers should be able to,
 GET all of the Order
 GET one Order
 GET Order with it's products
 GET Order with customer on order
 POST (Create) an Order type in the Orders table
 PUT (Update) an Order in the Orders table
 DELETE an Order and the associated products on the order (Not the products themselves)

 Deletion of a payment type is not allowed
*/

namespace BangazonAPI.Controllers
{
    // Sets the route and the _config variable for the database connection
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

        // GET: /Orders returns all orders
        // GET: /Orders?completed=false will return only the incomplete orders
        [HttpGet]
        public async Task<IActionResult> Get(string completed)
        {
            string sql = "SELECT * FROM Orders";

            if (completed != null && completed == "false")
            {
                sql += " WHERE CustomerAccountId IS null;";
            } else if (completed != null && completed == "true")
            {
                sql += " WHERE CustomerAccountId IS NOT null;";
            }

            using (IDbConnection conn = Connection)
            {
                var orders = await conn.QueryAsync<Order>(sql);
                return Ok(orders);
            }
        }

        // GET: /Orders/5 returns one order with the given id, specified by the number at end of URL
        // GET: /Orders/3?_include=products will return a given order and all the products on that order
        // GET: /Orders/3?_include=customers will return a given order and the assocaited customer
        [HttpGet("{id}", Name = "GetSingleOrder")]
        public async Task<IActionResult> Get([FromRoute] int id, string _include)
        {
            string sql = "";

            if (_include == null || ((_include.ToLower() != "products") && (_include.ToLower() != "customers")))
            {
                sql = $"SELECT * FROM Orders o WHERE o.Id = {id};";
            }
            else if (_include.ToLower() == "products")
            {
                sql = $@"SELECT *
                FROM Orders o
                JOIN OrderedProducts op on o.Id = op.OrderId
                JOIN Products p on op.ProductId = p.Id
                WHERE o.Id = {id};";
            } else if (_include.ToLower() == "customers")
            {
                sql = $@"SELECT *
                FROM Orders o
                JOIN Customers c on o.CustomerId = c.Id
                WHERE o.Id = {id};";
            }




            using (IDbConnection conn = Connection)
            {
                try
                {
                    if ((_include != null) && (_include.ToLower() == "products"))
                    {
                        Dictionary<string, Order> orderAndProducts = new Dictionary<string, Order>();

                        var orderPlusProducts = await conn.QueryAsync<Order, OrderedProduct, Product, Order>(sql, (order, orderedProduct, product) =>
                        {
                            string orderId = $"Order{(order.Id).ToString()}";
                            if (!orderAndProducts.ContainsKey(orderId))
                            {
                                orderAndProducts[orderId] = order;
                                orderAndProducts[orderId].Products.Add(product);
                            }
                            else
                            {
                                orderAndProducts[orderId].Products.Add(product);
                            }
                            return order;
                        });
                        return Ok(orderAndProducts);
                    }
                    else if ((_include != null) && (_include.ToLower() == "customers"))
                    {
                        var orderPlusCustomer = (await conn.QueryAsync<Order, Customer, Order>(sql, (order, customer) =>
                        {
                            order.Customer = customer;
                            return order;
                        })).Single();
                        return Ok(orderPlusCustomer);
                    }
                    else
                    {
                        var singleOrder = (await conn.QueryAsync<Order>(sql)).Single();
                        return Ok(singleOrder);
                    }
                }
                catch (InvalidOperationException)
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        // POST: /Orders
        // Creates an order in the Orders Table in the database, must supply CustomerId and CustomerAccountId in the body of the request as ints
        // Upon Order creation the customerAccountId will be set to null
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            order.CustomerAccountId = null;
            string sql = $@"INSERT INTO Orders
            (CustomerId)
            VALUES
            ('{order.CustomerId}');
            select MAX(Id) from Orders;";

            using (IDbConnection conn = Connection)
            {
                var createdOrder = (await conn.QueryAsync<int>(sql)).Single();
                order.Id = createdOrder;
                return CreatedAtRoute("GetSingleOrder", new { id = createdOrder }, order);
            }
        }

        // PUT: /Orders/5
        // Updates an order in the Orders Table in the database, must supply CustomerId and CustomerAccountId in the body of the request as ints
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

        // DELETE: /Orders/5 deletes a given order and the products associated with that order
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

        // Queries BangazonAPI DB to see if specific order exists. Returns true if it does, false if it doesn't
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
