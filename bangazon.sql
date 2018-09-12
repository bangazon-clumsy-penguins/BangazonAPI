
/* Company interface API */

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
    CONSTRAINT FK_Departments1 FOREIGN KEY(DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE EmployeeTrainings (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	TrainingId INT NOT NULL,
	CONSTRAINT FK_Employees2 FOREIGN KEY(EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK_Trainings2 FOREIGN KEY(TrainingId) REFERENCES Trainings(Id)
);

CREATE TABLE EmployeeComputers (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AssignmentDate DATE NOT NULL,
	ReturnDate DATE,
	EmployeeId INT NOT NULL,
	ComputerId INT NOT NULL,
	CONSTRAINT FK_Employees3 FOREIGN KEY(EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK_Computers3 FOREIGN KEY(ComputerId) REFERENCES Computers(Id)
);


/* Customer interface API */

CREATE TABLE ProductTypes (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Label VARCHAR NOT NULL
);

CREATE TABLE PaymentTypes (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Label VARCHAR NOT NULL
);

CREATE TABLE Customers (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR NOT NULL,
	LastName VARCHAR NOT NULL,
	JoinDate DATE NOT NULL,
	LastInteractionDate DATE NOT NULL
);

CREATE TABLE Products (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Title VARCHAR NOT NULL,
	Description VARCHAR NOT NULL,
	Quantity INTEGER NOT NULL,
	Price MONEY NOT NULL,
	ProductTypeId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	CONSTRAINT FK_ProductTypes4 FOREIGN KEY(ProductTypeId) REFERENCES ProductTypes(Id),
	CONSTRAINT FK_Customers4 FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE CustomerAccounts (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AccountNumber INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	PaymentTypeId INTEGER NOT NULL,
	CONSTRAINT FK_Customers5 FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
	CONSTRAINT FK_PaymentTypesId5 FOREIGN KEY(PaymentTypeId) REFERENCES PaymentTypes(Id)
);

CREATE TABLE Orders (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INTEGER NOT NULL,
	CustomerAccountId INTEGER,
	CONSTRAINT FK_Customers6 FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
	CONSTRAINT FK_CustomerAccounts6 FOREIGN KEY(CustomerAccountId) REFERENCES CustomerAccounts(Id)
);

CREATE TABLE OrderedProducts (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ProductId INTEGER NOT NULL,
	OrderId INTEGER NOT NULL,
	CONSTRAINT FK_Products7 FOREIGN KEY(ProductId) REFERENCES Products(Id),
	CONSTRAINT FK_Orders7 FOREIGN KEY(OrderId) REFERENCES Orders(Id)
);