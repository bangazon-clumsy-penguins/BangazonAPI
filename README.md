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







### 3. Payment Types
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


### 4. Products
Endpoint: [localhost:5000/Products](http://localhost:5000/Products)

Sample Product object:
```JSON
{
    "id": 1,
    "title": "Football",
    "description": "Sick Football",
    "quantity": 7,
    "price": 47.50,
    "productTypeId": 1,
    "customerId": 1
},
```

**GET**

Usage: Returns Product objects from the database.

GET /Products

- Returns an array of all Product objects in the database.

GET /Products/{id}

- Returns a single Product object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Products/5` returns the product type with the id of 5.

**POST**

Usage: Adds new Product objects to the database.

POST /Products

- Returns a JSON-formatted object representing the product type that was just posted.

Product objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "Title": "Name of Product",
    "Description": "Description of Product",
    "Quantity": 7,
    "Price": 47.50,
    "ProductTypeId": 1,
    "CustomerId": 1
}
```

The ProductTypeId property and the CustomerId property should both be integers corresponding to an existing ProductType/Customer resource.

**PUT**

Usage: Edits Product objects in the database.

PUT /Products/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Product object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "Title": "Name of Product",
    "Description": "Description of Product",
    "Quantity": 7,
    "Price": 47.50,
    "ProductTypeId": 1,
    "CustomerId": 1
}
```

The ProductTypeId property and the CustomerId property should both be integers corresponding to an existing ProductType/Customer resource.

**DELETE**

Usage: Removes Product objects from the database.

DELETE /Products/{id}

- Returns the HTTP status code "204 - No Content"

### 5. Product Types

Endpoint: [localhost:5000/Products](http://localhost:5000/ProductTypes)

Sample ProductTypes object:
```JSON
{
    "id": 1,
    "label": "Category of product (e.g. 'Electronics')"
}
```

**GET**

Usage: Returns ProductType objects from the database.

GET /ProductTypes

- Returns an array of all ProductType objects in the database.

GET /ProductTypes/{id}

- Returns a single ProductType object from the database with the "id" property equal to the {id} parameter that was passed. For example `/ProductTypes/5` returns the product type with the id of 5.

**POST**

Usage: Adds new ProductType objects to the database.

POST /ProductTypes

- Returns a JSON-formatted object representing the product type that was just posted.

ProductType objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
	"Label": "Category of product (e.g. 'Electronics')",
}
```

**PUT**

Usage: Edits ProductType objects in the database.

PUT /ProductTypes/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the ProductType object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
	"Label": "Category of product (e.g. 'Electronics')",
}
```

**DELETE**

Usage: Removes ProductType objects from the database.

DELETE /ProductTypes/{id}

- Returns the HTTP status code "204 - No Content"

## Corporate Resource Controllers

### 6. Computers

Endpoint: [localhost:5000/Computers](http://localhost:5000/Computers)

Sample Computer object:
```JSON
{
    "model": "PC",
    "purchaseDate": "2018-01-01T00:00:00",
    "decommissionDate": null
}
```

**GET**

Usate: Returns Computer objects from the database.

GET /Computers

- Returns an array of all Computer objects in the database.

GET /Computers/{id}

- Returns a single Training object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Computers/3` returns the computer with the id of 3.

**POST**

Usage: Adds new Computer objects to the database.

POST /Computers

- Returns a JSON-formatted object representing the computer that was just posted.

Computer objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "Model": "Model Name",
    "PurchaseDate": "YYYY-MM-DDT00:00:00",
    "DecommissionDate": null
}
```

**PUT**

Usage: Edits Computer objects in the database.

PUT /Computers/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Computer object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "Model": "Model Name",
    "PurchaseDate": "YYYY-MM-DDT00:00:00",
    "DecommissionDate": null
}
```

**DELETE**

Usage: Removes Computer objects from the database.

DELETE /Computers/{id}

- Returns the HTTP status code "204 - No Content"

### 7. Departments

Endpoint: [localhost:5000/Departments](http://localhost:5000/Departments)

Sample Department object:
```JSON
{
	"id": 1,
	"name": "Department Name",
	"budget": 4000.00,
	"employeeList": null
}
```

**GET**

Usage: Returns Department objects from the database.

GET /Departments 

- Returns an array of all Department objects in the database.

GET /Departments?\_include=employees 

- Returns an array of all Department objects with all the employees for each department included as an array of Employee objects in the "employeeList" property of each Department object.

GET /Departments/{id} 

- Returns a single Department object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Departments/5` returns the department with the id of 5.

**POST**

Usage: Adds new Department objects to the database.

POST /Departments

- Returns a JSON-formatted object representing the department that was just posted.

Department objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "Name": "Name of Department",
    "Budget": 5000.00
}
```

The Budget property should be a positive double.

**PUT**

Usage: 

/Departments/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Department object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "Name": "Name of Department",
    "Budget": 5000.00
}
```

The Budget property should be a positive double.

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
