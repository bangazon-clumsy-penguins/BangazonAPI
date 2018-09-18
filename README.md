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

Endpoint: [localhost:5000/Customers](http://localhost:5000/Customers)

Sample Customer object:
```JSON
{
    "id": 1,
    "firstName": "Tom",
    "lastName": "Smith",
    "joinDate": "2016-01-01T00:00:00",
    "lastInteractionDate": "2017-01-01T00:00:00",
    "customerProductsList": null,
    "customerAccountsList": null
},
```

**GET**

Usage: Returns Customer objects from the database.

GET /Customers
- Returns an array of all Customer objects in the database.

GET /Customers?_include=products

- Returns an array of all Customers in the database that have at least one Product associated with them. These products are included as an array of Product objects on the "customerProductList" property of each Customer. Basically, it shows all the customers who are currently selling products and what they are selling.

GET /Customers?_include=payments

- Returns an array of all Customers in the database that have completed at least one Order. The accounts that they used to pay for each of the orders are included as an array of CustomerAccount objects on the "customerAccountsList" property of each Customer. Basically, it shows all the customers who have purchased an order and which accounts they used to pay for their orders.

GET /Customers?_include=products,payments

- Returns an array of all Customer objects that are both: a.) selling at least one Product and b.) have paid for at least one order. It should include this information in the same manner as the individual 'products' and 'payments' queries listed above.

GET /Customers?active=false

- Returns an array of all Customers that have not placed any orders yet. This parameter overrides the `_include` queries, so it cannot be used in conjunction with them.

For example, the query string `?_include=products&active=false` will not actually return a list of all the Customers who are selling products, but haven't placed any orders yet. It will only show the customers who have not placed any orders yet, as if the `_include=products` query had not been entered.

GET /Customers?q={string}

- Returns an array of all Customers that have a FirstName or LastName property that includes the {string} parameter. For example, `/customers?q=john` might return customers with the first name of 'John' or the last name of 'Johnson'.

This query can be used in conjunction with all other queries to further narrow results. For example, `/customers?_include=products&q=es` will function as the `_include=products` query, except limited to customers with the string 'es' somewhere in their name.

GET /Customers/{id} 

- Returns a single Customer object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Customers/2` returns the customer with the id of 2.

**POST**

Usage: Adds new Customer objects to the database.

POST /Customers

- Returns a JSON-formatted object representing the customer that was just posted

Customer objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "FirstName": "First",
    "LastName": "Last",
    "JoinDate": "YYYY-MM-DDT00:00:00",
    "LastInteractionDate": "YYYY-MM-DDT00:00:00"
}
```

The LastInteractionDate property should not be earlier than the JoinDate property.

**PUT**

Usage: Edits a Customer object in the database.

PUT /Customers/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the Customer object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "FirstName": "First",
    "LastName": "Last",
    "JoinDate": "YYYY-MM-DDT00:00:00",
    "LastInteractionDate": "YYYY-MM-DDT00:00:00"
}
```

The LastInteractionDate property should not be earlier than the JoinDate property.

**DELETE**

There is no DELETE method for the Customers resource.

### 2. Orders

Endpoint: [localhost:5000/Orders](http://localhost:5000/Orders)

Sample Order object:
```JSON
{
    "products": [],
    "id": 1,
    "customerId": 1,
    "customerAccountId": 1,
    "customer": null
}
```

**GET**

Usage: Returns Order objects from the database.

GET /Orders 

- Returns an array of all Order objects in the database.

GET /Orders?completed=true

- Returns an array of all the order objects that are no longer "active", that is, they do not have a NULL CustomerAccountId. Using `completed=false` instead will return the array of orders that are still active.

GET /Orders/{id}

- Returns a single Order object from the database with the "id" property equal to the {id} parameter that was passed. For example `/Orders/5` returns the order with the id of 5.

GET /Orders/{id}?_include=products

- Returns the order corresponding to the {id} parameter and includes all the products on the order as an array of Product objects on the "products" property of the Order.

Example: 
```JSON
{
    "products": [
        { Product1 },
        { Product2 },
        { ... }
    ],
    "id": 1,
    "customerId": 1,
    "customerAccountId": 1,
    "customer": null
}
```

GET /Orders/{id}?_include=customers

- Returns the order corresponding to the {id} parameter and includes the customer who placed the order as a Customer object on the "customer" property of the Order.

Example:
```JSON
{
    "products": [],
    "id": 1,
    "customerId": 1,
    "customerAccountId": 1,
    "customer": {
        "id": 3,
        "firstName": "John",
        "lastName": "Smith",
        "joinDate": "2016-01-03T00:00:00",
        "lastInteractionDate": "2018-01-02T00:00:00",
        "customerProductsList": null,
        "customerAccountsList": null
    }
}
```

**POST**

Usage: Adds new Order objects to the database.

POST /Orders

- Returns a JSON-formatted object representing the order that was just posted.

Order objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
    "CustomerId": 3,
    "CustomerAccountId": null,
}
```
or just 
```JSON
{
    "CustomerId": 3
}
```

The CustomerId property should be an integer corresponding to an existing customer. The CustomerAccountId property will be set to NULL by default when an order is first posted.

The customer associated with the CustomerId on the order cannot have more than one "active" order (an order with a NULL CustomerAccountId property). If a new order is posted when a customer already has an active order, the new order will not be created and the HTTP status code "400 - Bad Request" will be returned.

**PUT**

Usage: Edits Order objects in the database.

PUT /Orders/{Id}

- Returns the HTTP status code "204 - No Content"

Like in the POST method, the Order object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
    "CustomerId": 3,
    "CustomerAccountId": 5
}
```

The CustomerAccountId property must be an integer and it must correspond
to an account on the CustomerAccounts table that matches the CustomerId of the customer on the order. Basically, when you update an order with PUT, you have to include the customer's payment method (CustomerAccount) and it has to be one of their own payment methods, not someone else's.

**DELETE**

Usage: Removes Order objects from the database.

DELETE /Orders/{id}

- Returns the HTTP status code "204 - No Content"

This method also removes all entries on the OrderedProducts intersection table that correspond to the order being deleted.






### 3. Payment Types
Endpoint: [localhost:5000/PaymentTypes](http://localhost:5000/PaymentTypes)

Sample PaymentTypes object:
```JSON
{
    "id": 1,
    "label": "Visa"
}
```

**GET**

Usage: Returns PaymentType objects from the database.

GET /PaymentTypes

- Returns an array of all PaymentType objects in the database.

GET /PaymentTypes/{id}

- Returns a single PaymentType object from the database with the "id" property equal to the {id} parameter that was passed. For example `/PaymentTypes/5` returns the product type with the id of 5.

**POST**

Usage: Adds new PaymentType objects to the database.

POST /PaymentTypes

- Returns a JSON-formatted object representing the payment type that was just posted.

PaymentType objects to be posted must be included in the body of the request and match the following JSON format:
```JSON
{
	"Label": "Type of payment (e.g. 'Visa')",
}
```

**PUT**

Usage: Edits PaymentType objects in the database.

PUT /PaymentTypes/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the PaymentType object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
	"Label": "Type of payment (e.g. 'Visa')",
}
```

**DELETE**

Usage: Removes PaymentType objects from the database.

DELETE /PaymentTypes/{id}

- Returns the HTTP status code "204 - No Content"


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

- Returns a JSON-formatted object representing the product that was just posted.

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

Endpoint: [localhost:5000/ProductTypes](http://localhost:5000/ProductTypes)

Sample ProductTypes object:
```JSON
{
    "id": 1,
    "label": "Electronics"
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
	"Label": "Category of product",
}
```

**PUT**

Usage: Edits ProductType objects in the database.

PUT /ProductTypes/{id}

- Returns the HTTP status code "204 - No Content"

Like the POST method, the ProductType object to be edited must be included in the body of the request and match the following JSON format:
```JSON
{
	"Label": "Category of product",
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
	"employeeList": []
}
```

**GET**

Usage: Returns Department objects from the database.

GET /Departments 

- Returns an array of all Department objects in the database.

GET /Departments?\_include=employees 

- Returns an array of all Department objects with all the employees for each department included as an array of Employee objects in the "employeeList" property of each Department object.

Example:
```JSON
{
	"id": 1,
	"name": "Department Name",
	"budget": 4000.00,
	"employeeList": [
        { Employee1 },
        { Employee1 },
        { ... }
    ]
}
```

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
		{ Employee1 },
		{ Employee2 },
		{ ... }
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
