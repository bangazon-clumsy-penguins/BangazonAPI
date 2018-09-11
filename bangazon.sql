/*

DELETE FROM EmployeeTrainings;
DELETE FROM EmployeeComputers;
DELETE FROM Employees;
DELETE FROM Departments;
DELETE FROM Computers;
DELETE FROM Trainings;

DELETE FROM OrderedProducts;
DELETE FROM Orders;
DELETE FROM Products;
DELETE FROM CustomerAccounts;
DELETE FROM ProductTypes;
DELETE FROM PaymentTypes;
DELETE FROM Customers;


ALTER TABLE EmployeeTrainings DROP CONSTRAINT [FK_Employees];
ALTER TABLE EmployeeTrainings DROP CONSTRAINT [FK_Trainings];
ALTER TABLE EmployeeComputers DROP CONSTRAINT [FK_Employees];
ALTER TABLE EmployeeComputers DROP CONSTRAINT [FK_Computers];
ALTER TABLE Employees DROP CONSTRAINT [FK_Departments];

ALTER TABLE OrderedProducts DROP CONSTRAINT [FK_Products];
ALTER TABLE OrderedProducts DROP CONSTRAINT [FK_Orders];
ALTER TABLE Orders DROP CONSTRAINT [FK_Customers];
ALTER TABLE Orders DROP CONSTRAINT [FK_CustomerAccounts];
ALTER TABLE Products DROP CONSTRAINT [FK_ProductTypes];
ALTER TABLE Products DROP CONSTRAINT [FK_Customers];
ALTER TABLE CustomerAccounts DROP CONSTRAINT [FK_Customers];
ALTER TABLE CustomerAccounts DROP CONSTRAINT [FK_PaymentTypes];


DROP TABLE IF EXISTS EmployeeTrainings;
DROP TABLE IF EXISTS EmployeeComputers;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Departments;
DROP TABLE IF EXISTS Computers;
DROP TABLE IF EXISTS Trainings;

DROP TABLE IF EXISTS OrderedProducts;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS CustomerAccounts;
DROP TABLE IF EXISTS ProductTypes;
DROP TABLE IF EXISTS PaymentTypes;
DROP TABLE IF EXISTS Customers;

*/

/*
CREATE TABLE Instructor (
    Id	INTEGER NOT NULL PRIMARY KEY IDENTITY,
    FirstName	varchar(80) NOT NULL,
    LastName	varchar(80) NOT NULL,
    SlackHandle	varchar(80) NOT NULL,
    Specialty varchar(80),
    CohortId	INTEGER NOT NULL,
    CONSTRAINT FK_Cohort FOREIGN KEY(CohortId) REFERENCES Cohort(Id)
);
*/

CREATE TABLE Trainings (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	MaxOccupancy INT NOT NULL
);

CREATE TABLE Computers (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Model VARCHAR NOT NULL,
	PurchaseDate DATE NOT NULL,
	DecommissionDate DATE
);

CREATE TABLE Departments (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR NOT NULL,
	Budget MONEY NOT NULL
);

CREATE TABLE Employees (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR NOT NULL,
	LastName VARCHAR NOT NULL,
	HireDate DATE NOT NULL,
	IsSupervisor BIT NOT NULL,
	DepartmentId INT NOT NULL,
    CONSTRAINT FK_Departments FOREIGN KEY(DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE EmployeeTrainings (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	TrainingId INT NOT NULL
);