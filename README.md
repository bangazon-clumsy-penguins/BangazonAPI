
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