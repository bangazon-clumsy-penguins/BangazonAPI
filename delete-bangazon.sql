/* SQL script to delete information from all tables, drop FK constraints, and drop all tables */

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
*/

ALTER TABLE EmployeeTrainings DROP CONSTRAINT [FK_Employees2];
ALTER TABLE EmployeeTrainings DROP CONSTRAINT [FK_Trainings2];
ALTER TABLE EmployeeComputers DROP CONSTRAINT [FK_Employees3];
ALTER TABLE EmployeeComputers DROP CONSTRAINT [FK_Computers3];
ALTER TABLE Employees DROP CONSTRAINT [FK_Departments1];

ALTER TABLE OrderedProducts DROP CONSTRAINT [FK_Products7];
ALTER TABLE OrderedProducts DROP CONSTRAINT [FK_Orders7];
ALTER TABLE Orders DROP CONSTRAINT [FK_Customers6];
ALTER TABLE Orders DROP CONSTRAINT [FK_CustomerAccounts6];
ALTER TABLE Products DROP CONSTRAINT [FK_ProductTypes4];
ALTER TABLE Products DROP CONSTRAINT [FK_Customers4];
ALTER TABLE CustomerAccounts DROP CONSTRAINT [FK_Customers5];
ALTER TABLE CustomerAccounts DROP CONSTRAINT [FK_PaymentTypes5];


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