# The Bangazon Platform API (BangazonAPI)
Repo for first Bangazon sprint.

## Table of Contents

### Client Resource Controllers
1. Customers
1. Orders
1. PaymentTypes
1. Products
1. ProductTypes

### Corporate Resource Controllers
6. Computers
1. Departments
1. Employees
1. Trainings

## Client Resource Controllers

### 1. Customers

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

```
JSON
{
    "firstName": "Tom",
    "lastName": "Smith",
    "lastInteractionDate": "2017-01-01T00:00:00"
}
```

**DELETE**

Usage: /Customers/{Id}

Delete a customer matching the supplied Id


### 2. Orders
**GET**

Endpoint: [localhost:5000/Orders](http://localhost:5000/Orders)

Usage:

/Orders - return array of all Order objects

/Orders?(_include=products, _include=customer) returns an Order with the matching parameter inside the Order as a List(products) or Object(customer)
/Orders?(completed=false, completed=true) returned only the incomplete or complete orders. Complete orders are those with a customerAccountId
/Orders/{Id} returns a single object matching the Id

**POST**

Must match Order model. CustomerId and CustomerAccountId must be passed.
Post will only function if the customer has no active orders (CustomerAccountId = null is an active order).
If customer does have an active order a 400 status code (Bad Request) will be thrown.

```JSON
{
    "CustomerId": 3,
    "CustomerAccountId": null
}
```
or 

```JSON
{
    "CustomerId": 3
}```

**PUT**

Usage: /Orders/{Id}

Edit a Order matching the supplied Id

Must match Order model. CustomerId and CustomerAccountId must be passed.

```JSON
{
    "CustomerId": 3,
    "CustomerAccountId": 5
}
```

**DELETE**
Usage: /Orders/{Id}

Delete an Order matching the supplied Id and the products on the order







### 6. Payment Types
**GET**

Endpoint: [localhost:5000/paymentTypes](http://localhost:5000/paymentTypes)

Usage:

/paymentTypes - return array of all paymentTypes objects

/paymentTypes/{Id} returns a single object matching the Id

**POST**

Must match PaymentType model. Label must be passed.

```JSON
{
    "Label": "Visa"
}
```
**PUT**

Usage: /PaymentTypes/{Id}

Edit a PaymentType matching the supplied Id

Must match PaymentType model. Label must be passed.

```JSON
{
    "CustomerId": "Master Card"
}
```


### 2. Products

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




### 4. Product Types

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

To delete a product type, make a DELETE request to URL:

```
http://localhost:5000/ProductTypes/7
```

Where '7' is the Id of the product type to delete



## Corporate Resource Controllers

### 6. Computers

**GET**

To get all Computers, make a GET request to URL:
```
http://localhost:5000/Computers
```
Returned will be an array of:

```JSON
[
    {
        "model": "PC",
        "purchaseDate": "2018-01-01T00:00:00",
        "decommissionDate": null
    }
]
```

To get a single Computer, add a /{id} to the GET request URL:

```
http://localhost:5000/Computers/7
```

Returned will be a single Product Type of:

```JSON
{
        "id": 1,
        "model": "PC",
        "purchaseDate": "2018-01-01T00:00:00",
        "decommissionDate": null
 }
```

**POST**

To add a new product type, make a POST request to URL:

```
http://localhost:5000/Computers
```

With a request body in the form:

```JSON
    {
        "model": "PC",
        "purchaseDate": "2018-01-01T00:00:00",
        "decommissionDate": null
    }
```

**PUT**

To update a product type, make a PUT request to URL:

```
http://localhost:5000/Computers/7
```

Where '7' is the Id of the product type to update,
With a request body containing the updated information:

```JSON
{
    {
        
        "model": "PC",
        "purchaseDate": "2018-01-01T00:00:00",
        "decommissionDate": null
    }
}
```

To delete a Computer, make a DELETE request to URL:

```
http://localhost:5000/Computer/7
```

Where '7' is the Id of the Computer to delete

### 7. Departments

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

### 8. Employees

Endpoint: [localhost:5000/Employees](http://localhost:5000/Employees)

Sample Employee object:
```JSON
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
```

**GET**

Usage: Returns Employee objects from the database.

GET /Employees

- Returns an array of all Employee objects in the database, with each employee's department and computer included as properties on the Employee object.

GET /Employees/{id}

- Returns a single Employee object from the database with the "id" property matching the {id} parameter that was passed. For example `/Employees/5` returns the employee with the id of 5. Also, the employee's department and computer are included as properties on the Employee object.

**POST**

Usage: Adds new Employee objects to the database.

POST /Employees

- Returns a JSON-formatted object representing the employee that was just posted.

Employee objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "FirstName": "John",
    "LastName": "Smith",
    "HireDate": "YYYY-MM-DDT00:00:00",
    "IsSupervisor": false,
    "DepartmentId": 1
}
```

The DepartmentId property should be an integer corresponding to an existing department and the IsSupervisor property is a boolean that should false if the department already has a supervisor.

**PUT**

Usage: Edits Employee objects in the database.

PUT /Employees/{id}

- Returns the HTTP status code "204 - No Content"

Like in the POST method, the Employee object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "FirstName": "John",
    "LastName": "Smith",
    "HireDate": "YYYY-MM-DDT00:00:00",
    "IsSupervisor": false,
    "DepartmentId": 1
}
```

The DepartmentId property should be an integer corresponding to an existing department and the IsSupervisor property is a boolean that should be false if the department already has a supervisor.


### 9. Trainings

Endpoint: [localhost:5000/Trainings](http://localhost:5000/Trainings)

Sample Training object:
```JSON
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
```

**GET**

Usage: Returns Training objects from the database.

GET /Trainings

- Returns an array of all Training objects in the database, with all the employees registered for each training included as an array of Employee objects on each Training object.

GET /Trainings/{id}

- Returns a single Training object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Trainings/5` returns the training with the id of 5. Also, all the employees registered for that training are included as an array of Employee objects on the Training object.

GET /Trainings?completed=false

- Returns an array of Trainings objects with "endDate" properties of the current day or later.


**POST**

Usage: Adds new Training objects to the database.

POST /Trainings

- Returns a JSON-formatted object representing the training that was just posted.

Training objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
	"Name": "Name of Training",
	"StartDate": "YYYY-MM-DDT00:00:00",
	"EndDate": "YYYY-MM-DDT00:00:00",
	"MaxOccupancy": 42
}
```

The MaxOccupancy property must be a positive integer and the EndDate property must not be before the StartDate. Otherwise, an exception will be thrown and the item will not be created.

**PUT**

Usage: Edits Training objects in the database.

PUT /Trainings/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Training object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
	"Name": "Name of Training",
	"StartDate": "YYYY-MM-DDT00:00:00",
	"EndDate": "YYYY-MM-DDT00:00:00",
	"MaxOccupancy": 42
}
```

The MaxOccupancy property must be a positive integer and the EndDate property must not be before the StartDate. Otherwise, an exception will be thrown and the item will not be edited.

**DELETE**

Usage: Removes Training objects from the database.

DELETE /Trainings/{id}

- Returns the HTTP status code "204 - No Content"

The "StartDate" of the training to be deleted must be in the future. Otherwise, an exception will be thrown and the item will not be deleted
