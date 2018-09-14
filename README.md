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

# BangazonAPI

Repo for first Bangazon sprint

## Employees

### Get All Employees:

To get all employees, make a GET request to URL:

```
http://localhost:5000/Employees
```

Returned will be an array of:

```
[
    {
        "id": 1,
        "firstName": "Tommy",
        "lastName": "Smith",
        "hireDate": "2015-05-01T00:00:00",
        "isSupervisor": true,
        "departmentId": 1,
        "department": {
            "id": 1,
            "name": "Finance",
            "budget": 4000
        },
        "computer": {
            "id": 5,
            "model": "PC",
            "purchaseDate": "2018-01-05T00:00:00",
            "decommisionDate": null
        }
    }
]
```

### Get Single Employee:

To get a single employee, add a /{id} to the GET request URL:

```
http://localhost:5000/Employees/7
```

Returned will be a single Employee of:

```
{
    "id": 7,
    "firstName": "John",
    "lastName": "Williams",
    "hireDate": "2015-05-01T00:00:00",
    "isSupervisor": true,
    "departmentId": 1,
    "department": {
        "id": 1,
        "name": "Finance",
        "budget": 4000
    },
    "computer": null
}
```

### Add Employee:

To add an employee, make a POST request to URL:

```
http://localhost:5000/Employees
```

With a request body in the form:

```
    {
        "firstName": "Jimmy",
        "lastName": "Little",
        "hireDate": "2015-05-01T00:00:00",
        "isSupervisor": true,
        "departmentId": 1
    }
```

### Update Employee:

To update an employees information, make a PUT request to URL:

```
http://localhost:5000/Employees/7
```

Where '7' is the Id of the employee to update,
With a request body containing the updated information:

```
    {
        "firstName": "Sarah",
        "lastName": "Blackmon",
        "hireDate": "2015-05-01T00:00:00",
        "isSupervisor": true,
        "departmentId": 1
    }
```
