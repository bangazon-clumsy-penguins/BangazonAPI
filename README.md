# Building the Bangazon Platform API

Welcome, new Bangazonians!

Your job is to build out a .NET Web API that makes each resource in the Bangazon ERD available to application developers throughout the entire company.

1. Products
1. Product types
1. Customers
1. Orders
1. Payment types
1. Employees
1. Computers
1. Training programs
1. Departments

> **Pro tip:** You do not need to make a Controller for the join tables, because those aren't resources.

Your product owner will provide you with a prioritized backlog of features for you to work on over the development sprint. The first version of the API will be completely open since we have not determined which authentication method we want to use yet.

The only restriction on the API is that only requests from the `www.bangazon.com` domain should be allowed. Requests from that domain should be able to access every resource, and perform any operation a resource.

## Plan

First, you need to plan. Your team needs to come to a consensus about the Bangazon ERD design. Once you feel you have consensus, you must get it approved by your manager before you begin writing code for the API.

## Modeling

Next, you need to author the Models needed for your API. Make sure that each model has the approprate foreign key relationship defined on it, either with a custom type or an `List<T>` to store many related things. The boilerplate code shows you one example - the relationship between `Order` and `OrderProduct`, which is 1 -> &#8734;. For every _OrderId_, it can be stored in the `OrderProduct` table many times.

## Database Management

Your team will need to decide on a file to be added to your repository to contain all of the SQL needed to build and seed your database. Perhaps a file named `bangazon.sql`.

If your database needs to be changed in any way, or you wish to add items to be seeded, the a teammate will need to modify the file, submit a PR, and each teammate will need to run it to rebuild the database with the new structure.

## Controllers

Now it's time to build the controllers that handle GET, POST, PUT, and DELETE operations on each resource. Make sure you read, and understand, the requirements in the issue tickets to you can use your ORM and SQL to return the correct data structure to client requests.

### 1. Customers Controller
**GET**

Endpoint: [localhost:5000/Customers](http://localhost:5000/Customers)

Usage: 

/Customers - return array of all customer objects

/Customers?(active=false, _include=products, _include=payments, _include=products,payments, q=SearchString) returns an array of objects matching the parameters. The "active" parameter overrides all other parameters except q.

/Customers/{Id} returns a single object matching the Id

**POST**

Must match Customer model. FirstName, LastName, JoinDate, and LastInteractionDate must be passed.

```JSON
{
    "firstName": "Tom",
    "lastName": "Smith",
    "joinDate": "2016-01-01T00:00:00",
    "lastInteractionDate": "2017-01-01T00:00:00"
}
```
**PUT**

Usage: /Customers/{Id}

Edit a customer matching the supplied Id

Must match Customer model. FirstName, LastName, and LastInteractionDate are required params.

```JSON
{
    "firstName": "Tom",
    "lastName": "Smith",
    "lastInteractionDate": "2017-01-01T00:00:00"
}
```

**DELETE**

Usage: /Customers/{Id}

Delete a customer matching the supplied Id

### 2. Products Controller
**GET**

Endpoint: [localhost:5000/Products](http://localhost:5000/Products)

Usage: 

/Products - return array of all customer objects

returns an array of objects

/Products/{Id} returns a single object matching the Id

**POST**

Must match Product model. Title, Description, Quantity, Price, ProductTypeId, and CustomerId are required params.

```JSON
{
    "title": "Football",
    "description": "Sick Football",
    "quantity": 7,
    "price": 47.5,
    "productTypeId": 1,
    "customerId": 1
}
```
**PUT**

Usage: /Products/{Id}

Edit a product matching the supplied Id

Must match Product model. Title, Description, Quantity, Price, ProductTypeId, and CustomerId are required params.

```JSON
{
    "title": "Football",
    "description": "Sick Football",
    "quantity": 7,
    "price": 47.5,
    "productTypeId": 1,
    "customerId": 1
}
```

**DELETE**

Usage: /Products/{Id}

Delete a product matching the supplied Id

