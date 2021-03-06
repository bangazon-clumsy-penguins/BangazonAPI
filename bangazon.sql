
/* Company interface API */

CREATE TABLE Trainings (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR(80) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	MaxOccupancy INT NOT NULL
);

CREATE TABLE Computers (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Model VARCHAR(80) NOT NULL,
	PurchaseDate DATE NOT NULL,
	DecommissionDate DATE
);

CREATE TABLE Departments (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name VARCHAR(80) NOT NULL,
	Budget MONEY NOT NULL
);

CREATE TABLE Employees (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(80) NOT NULL,
	LastName VARCHAR(80) NOT NULL,
	HireDate DATE NOT NULL,
	IsSupervisor BIT NOT NULL,
	DepartmentId INT NOT NULL,
    CONSTRAINT FK_Departments1 FOREIGN KEY(DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE EmployeeTrainings (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	TrainingId INT NOT NULL,
	CONSTRAINT FK_Employees2 FOREIGN KEY(EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK_Trainings2 FOREIGN KEY(TrainingId) REFERENCES Trainings(Id)
);

CREATE TABLE EmployeeComputers (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	AssignmentDate DATE NOT NULL,
	ReturnDate DATE,
	EmployeeId INT NOT NULL,
	ComputerId INT NOT NULL,
	CONSTRAINT FK_Employees3 FOREIGN KEY(EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK_Computers3 FOREIGN KEY(ComputerId) REFERENCES Computers(Id)
);


/* Customer interface API */

CREATE TABLE ProductTypes (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Label VARCHAR(80) NOT NULL
);

CREATE TABLE PaymentTypes (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Label VARCHAR(80) NOT NULL
);

CREATE TABLE Customers (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(80) NOT NULL,
	LastName VARCHAR(80) NOT NULL,
	JoinDate DATE NOT NULL,
	LastInteractionDate DATE NOT NULL
);

CREATE TABLE Products (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Title VARCHAR(80) NOT NULL,
	Description VARCHAR(80) NOT NULL,
	Quantity INT NOT NULL,
	Price MONEY NOT NULL,
	ProductTypeId INT NOT NULL,
	CustomerId INT NOT NULL,
	CONSTRAINT FK_ProductTypes4 FOREIGN KEY(ProductTypeId) REFERENCES ProductTypes(Id),
	CONSTRAINT FK_Customers4 FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE CustomerAccounts (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	AccountNumber INT NOT NULL,
	CustomerId INT NOT NULL,
	PaymentTypeId INT NOT NULL,
	CONSTRAINT FK_Customers5 FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
	CONSTRAINT FK_PaymentTypes5 FOREIGN KEY(PaymentTypeId) REFERENCES PaymentTypes(Id)
);

CREATE TABLE Orders (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INT NOT NULL,
	CustomerAccountId INT,
	CONSTRAINT FK_Customers6 FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
	CONSTRAINT FK_CustomerAccounts6 FOREIGN KEY(CustomerAccountId) REFERENCES CustomerAccounts(Id)
);

CREATE TABLE OrderedProducts (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	ProductId INT NOT NULL,
	OrderId INT NOT NULL,
	CONSTRAINT FK_Products7 FOREIGN KEY(ProductId) REFERENCES Products(Id),
	CONSTRAINT FK_Orders7 FOREIGN KEY(OrderId) REFERENCES Orders(Id)
);