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

### 1. Trainings Controller

Endpoint: [localhost:5000/Trainings](http://localhost:5000/Trainings)

Sample Training object:
````JSON
{
	"registeredEmployees": [
		{Employee1},
		{Employee2},
		{...}
	],
	"id": 1,
	"name": "Very Important Training",
	"startDate": "2018-09-14T00:00:00",
	"endDate": "2018-09-21T00:00:00",
	"maxOccupancy": 5
}
````

**GET**

Usage: Returns Training objects from the database.

GET /Trainings

- Returns an array of all Training objects, with all the employees registered for each training included as an array of Employee objects on each Training object.

GET /Trainings/{Id}

- Returns a single Training object with the "id" property equal to the {Id} parameter that was passed. Also, all the employees registered for that training are included as an array of Employee objects on the Training object.

GET /Trainings?completed=false

- Returns an array of Trainings objects with "endDate" properties of the current day or later.


**POST**

Usage: Adds new Training objects to the database.

POST /Trainings

- Returns a JSON-formatted object representing the training that was just posted.

Training objects to be posted must be included in the body of the request and match the following JSON format:
````JSON
{
	"Name": "Name of Training",
	"StartDate": "YYYY-MM-DDT00:00:00",
	"EndDate": "YYYY-MM-DDT00:00:00",
	"MaxOccupancy": 42
}
````

The MaxOccupancy property must be a positive integer. Otherwise, an exception will be thrown and the item will not be posted.

**PUT**

Usage: Edits Training objects in the database.

PUT /Trainings/{Id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Training object to be edited must be included in the body of the request and match the following JSON format:
````JSON
{
	"Name": "Name of Training",
	"StartDate": "YYYY-MM-DDT00:00:00",
	"EndDate": "YYYY-MM-DDT00:00:00",
	"MaxOccupancy": 42
}
````

The MaxOccupancy property must be a positive integer. Otherwise, an exception will be thrown and the item will not be edited.

**DELETE**

Usage: Removes Training objects from the database.

DELETE /Trainings/{Id}

- Returns the HTTP status code "204 - No Content"

The "StartDate" of the training to be deleted must be in the future. Otherwise, an exception will be thrown and the item will not be deleted

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

## 3. Product Types

**GET**

To get all product types, make a GET request to URL:

```
http://localhost:5000/ProductTypes
```

Returned will be an array of:

```JSON
[
    {
        "id": 1,
        "label": "Balls"
    }
]
```

To get a single product type, add a /{id} to the GET request URL:

```
http://localhost:5000/ProductTYpes/7
```

Returned will be a single Product Type of:

```JSON
{
    "id": 7,
    "label": "Shoes"
}
```

**POST**

To add a new product type, make a POST request to URL:

```
http://localhost:5000/ProductTypes
```

With a request body in the form:

```JSON
    {
        "label": "Knives"
    }
```

**PUT**

To update a product type, make a PUT request to URL:

```
http://localhost:5000/ProductTypes/7
```

Where '7' is the Id of the product type to update,
With a request body containing the updated information:

```JSON
{
    "label": "UpdatedCategoryName"
}
```

**DELETE**

To delete a product type, make a DELETE request to URL:

```
http://localhost:5000/ProductTypes/7
```

Where '7' is the Id of the product type to delete

## 4. Employees

**GET**

To get all employees, make a GET request to URL:

```
http://localhost:5000/Employees
```

Returned will be an array of:

```JSON
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

To get a single employee, add a /{id} to the GET request URL:

```
http://localhost:5000/Employees/7
```

Returned will be a single Employee of:

```JSON
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

**POST**

To add an employee, make a POST request to URL:

```
http://localhost:5000/Employees
```

With a request body containing the employee information:

```JSON
    {
        "firstName": "Sarah",
        "lastName": "Blackmon",
        "hireDate": "2015-05-01T00:00:00",
        "isSupervisor": true,
        "departmentId": 1
    }
```

**PUT**

To update an employees information, make a PUT request to URL:

```
http://localhost:5000/Employees/7
```

Where '7' is the Id of the employee to update,
With a request body containing the updated information:

```JSON
    {
        "firstName": "SarahUpdated",
        "lastName": "BlackmonUpdated",
        "hireDate": "2015-05-01T00:00:00",
        "isSupervisor": true,
        "departmentId": 1
    }
```
### 5. Departments Controller

**GET**

Endpoint: [localhost:5000/Departments](http://localhost:5000/Departments)

Usage:

/Departments - return array of all department objects

/Customers?\_include=employees - returns all department objects with a list of employees

/Departments/{Id} returns a single object matching the Id

**POST**

Must match Department model. Name, Budget.

```JSON
{
    "Name": "Finance",
    "Budget": 56000
}
```

**PUT**

Usage: /Departments/{Id}

Edit a customer matching the supplied Id

Must match Department model. Name, Budget.

```JSON
{
    "Name": "Finance",
    "Budget": 56000
}
```


