-- P01--
CREATE DATABASE Minions

--P02--
USE Minions

CREATE TABLE Minions
(
	Id INT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	Age SMALLINT
)

ALTER TABLE Minions
ADD PRIMARY KEY (Id)

CREATE TABLE Towns
(
	Id INT NOT NULL,
	[Name] VARCHAR(50) NOT NULL
)

ALTER TABLE Towns
ADD PRIMARY KEY (Id)

--P03--
ALTER TABLE Minions
ADD TownId INT

ALTER TABLE Minions
ADD FOREIGN KEY (TownId) REFERENCES Towns(Id)

--P04--
INSERT INTO Towns (Id, [Name]) VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO Minions (Id, [Name], Age, TownId) VALUES
(1, 'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', NULL, 2)

--P05--
DELETE FROM Minions

--P06--
DROP TABLE Minions
DROP TABLE Towns

--P07--
CREATE TABLE People
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3,2),
	[Weight] DECIMAL(5,2),
	Gender CHAR(1) CHECK(Gender = 'm' or Gender = 'f') NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People([Name], Picture, Height, [Weight], Gender, Birthdate, Biography) VALUES
('Name1', NULL, 1.00, 100.00, 'm', '2020-12-31', NULL),
('Name2', NULL, 2.00, 110.00, 'm', '2021-12-31', NULL),
('Name3', NULL, 3.00, 120.00, 'm', '2022-12-31', NULL),
('Name4', NULL, 4.00, 130.00, 'f', '2023-12-31', NULL),
('Name5', NULL, 5.00, 140.00, 'f', '2024-12-31', NULL)

--P08--
CREATE TABLE Users
(
	Id BIGINT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	LastLoginTime DATETIME2 NOT NULL,
	IsDeleted BIT NOT NULL 
)

INSERT INTO Users(Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES
('NameA', '1234561', NULL, GETDATE(), 0),
('NameB', '1234562', NULL, GETDATE(), 0),
('NameC', '1234563', NULL, GETDATE(), 0),
('NameD', '1234564', NULL, GETDATE(), 1),
('NameE', '1234565', NULL, GETDATE(), 1)

--P09--
ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC0711B83DA6

ALTER TABLE Users
ADD CONSTRAINT PK_IdUsername PRIMARY KEY(Id, Username)

--P10--
ALTER TABLE Users
ADD CONSTRAINT CH_Password CHECK(DATALENGTH([Password]) >= 5)

--P11--
ALTER TABLE Users
ADD CONSTRAINT DF_LastLoginTime DEFAULT GETDATE() FOR LastLoginTime

--P12--
ALTER TABLE Users
DROP PK_IdUsername

ALTER TABLE Users
ADD CONSTRAINT PK_Id PRIMARY KEY (Id)

ALTER TABLE Users
ADD CONSTRAINT UQ_Username UNIQUE (Username)

ALTER TABLE Users
ADD CONSTRAINT CH_Username CHECK (DATALENGTH(Username) >= 3)

--P13--
CREATE DATABASE Movies
USE Movies

CREATE TABLE Directors
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	DirectorName VARCHAR(50) NOT NULL,
	Notes VARCHAR(500)
)

INSERT INTO Directors(DirectorName, Notes) VALUES
('Name1', NULL),
('Name2', NULL),
('Name3', NULL),
('Name4', NULL),
('Name5', NULL)

CREATE TABLE Genres
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	GenreName VARCHAR(50) NOT NULL,
	Notes VARCHAR(500)
)

INSERT INTO Genres(GenreName, Notes) VALUES
('GenreName1', NULL),
('GenreName2', NULL),
('GenreName3', NULL),
('GenreName4', NULL),
('GenreName5', NULL)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CategoryName VARCHAR(50) NOT NULL,
	Notes VARCHAR(500)
)

INSERT INTO Categories(CategoryName, Notes) VALUES
('CategoryName1', NULL),
('CategoryName2', NULL),
('CategoryName3', NULL),
('CategoryName4', NULL),
('CategoryName5', NULL)

CREATE TABLE Movies
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Title NVARCHAR(300) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear DATE NOT NULL,
	[Length] SMALLINT NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	RATING DECIMAL(2,1),
	Notes VARCHAR(500)
)

INSERT INTO Movies(Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, RATING, Notes) VALUES
('Title1', 1, '2010', 90, 1, 1, 4.0, NULL),
('Title1', 2, '2011', 91, 2, 2, 4.2, NULL),
('Title1', 3, '2012', 92, 3, 3, 4.3, NULL),
('Title1', 4, '2013', 93, 4, 4, 4.4, NULL),
('Title1', 5, '2014', 94, 5, 5, 4.5, NULL)

--P14--
CREATE DATABASE CarRental

USE CarRental

--Categories (Id, CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate)
CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CategoryName VARCHAR(50) NOT NULL,
	DailyRate DECIMAL(2,1),
	WeeklyRate DECIMAL(2,1),
	MonthlyRate DECIMAL(2,1),
	WeekendRate DECIMAL(2,1)
)

INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate) VALUES
('CategoryName1', NULL, NULL, NULL, NULL),
('CategoryName2', NULL, NULL, NULL, NULL),
('CategoryName3', NULL, NULL, NULL, NULL)

--Cars (Id, PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	PlateNumber SMALLINT NOT NULL,
	Manufacturer NVARCHAR(50) NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	CarYear DATE NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Doors TINYINT NOT NULL,
	Picture VARCHAR(MAX) NOT NULL,
	Condition VARCHAR(50) NOT NULL,
	Available VARCHAR(50) NOT NULL
)

INSERT INTO Cars(PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available) VALUES
(561, 'Manufacturer1', 'Model1', '2010', 1, 3, 'PictureURL1', 'Condition1', 'YES'),
(562, 'Manufacturer2', 'Model2', '2011', 2, 4, 'PictureURL2', 'Condition2', 'YES'),
(563, 'Manufacturer3', 'Model3', '2012', 3, 5, 'PictureURL3', 'Condition3', 'NO')

--Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes VARCHAR(200)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) VALUES
('FirstName1', 'LastName1', 'Job1', NULL),
('FirstName2', 'LastName2', 'Job2', NULL),
('FirstName3', 'LastName3', 'Job3', NULL)

--Customers (Id, DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
CREATE TABLE Customers
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	DriverLicenceNumber SMALLINT NOT NULL,
	FullName VARCHAR(100) NOT NULL,
	[Address] VARCHAR(100) NOT NULL,
	City VARCHAR(50) NOT NULL,
	ZIPCode SMALLINT NOT NULL,
	Notes VARCHAR(200)
)

INSERT INTO Customers (DriverLicenceNumber, FullName, [Address], City, ZIPCode, Notes) VALUES
(12345, 'FullName1', 'Address1', 'City1', 1000, NULL),
(12347, 'FullName2', 'Address2', 'City2', 1001, NULL),
(12348, 'FullName3', 'Address3', 'City3', 1002, NULL)

--RentalOrders (Id, EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate, OrderStatus, Notes)
CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES CarS(Id) NOT NULL,
	TankLevel VARCHAR(30) NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT NOT NULL,
	StartDate DATETIME2 NOT NULL,
	EndDate DATETIME2 NOT NULL,
	TotalDays DATE NOT NULL,
	RateApplied DECIMAL(6,2) NOT NULL,
	TaxRate DECIMAL(6,2) NOT NULL,
	OrderStatus VARCHAR(40) NOT NULL,
	Notes VARCHAR(200)
)

INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate, OrderStatus, Notes) VALUES
(1, 2, 3, 'FULL', 0, 100, 100, '2020-10-20', '2020-10-25', '2020-10-25', 4.5, 5.5, 'READY', NULL),
(2, 3, 4, 'FULL', 1, 101, 100, '2020-11-20', '2020-11-25', '2020-10-25', 4.5, 5.5, 'READY', NULL),
(3, 4, 5, 'FULL', 2, 102, 100, '2020-12-20', '2020-12-25', '2020-10-25', 4.5, 5.5, 'READY', NULL)

--P15--
CREATE DATABASE Hotel

USE Hotel

--Employees (Id, FirstName, LastName, Title, Notes)
CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO Employees(FirstName, LastName, Title, Notes) VALUES
('FirstName1', 'LastName1', 'Title1', NULL),
('FirstName2', 'LastName2', 'Title2', NULL),
('FirstName3', 'LastName3', 'Title3', NULL)

--Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	PhoneNumber CHAR(10),
	EmergencyName VARCHAR(30) NOT NULL,
	EmergencyNumber CHAR(10) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO Customers(FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes) VALUES
('FirstName1', 'LastName1', NULL, 'EmergencyName1', 1234567890, NULL),
('FirstName2', 'LastName2', NULL, 'EmergencyName2', 1234567890, NULL),
('FirstName3', 'LastName3', NULL, 'EmergencyName3', 1234567890, NULL)

--RoomStatus (RoomStatus, Notes)
CREATE TABLE RoomStatus
(
	Id INT PRIMARY KEY IDENTITY,
	RoomStatus VARCHAR(40) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO RoomStatus(RoomStatus, Notes) VALUES
('RoomStatus1', NULL),
('RoomStatus2', NULL),
('RoomStatus3', NULL)

--RoomTypes (RoomType, Notes)
CREATE TABLE RoomTypes
(
	Id INT PRIMARY KEY IDENTITY,
	RoomType VARCHAR(40) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO RoomTypes(RoomType, Notes) VALUES
('RoomType1', NULL),
('RoomType2', NULL),
('RoomType3', NULL)

--BedTypes (BedType, Notes)
CREATE TABLE BedTypes
(
	Id INT PRIMARY KEY IDENTITY,
	BedType VARCHAR(40) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO BedTypes(BedType, Notes) VALUES
('BedType1', NULL),
('BedType2', NULL),
('BedType3', NULL)

--Rooms (RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY IDENTITY,
	RoomType INT FOREIGN KEY REFERENCES RoomTypes(Id) NOT NULL,
	BedType INT FOREIGN KEY REFERENCES BedTypes(Id) NOT NULL,
	Rate DECIMAL(2,1),
	RoomStatus INT FOREIGN KEY REFERENCES RoomStatus(Id) NOT NULL,
	Notes VARCHAR(100)
)

INSERT INTO Rooms(RoomType, BedType, Rate, RoomStatus, Notes) VALUES
(1, 1, 4.1, 1, NULL),
(2, 2, 4.2, 2, NULL),
(3, 3, 4.3, 3, NULL)

--Payments (Id, EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes)
CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATETIME2 NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATETIME2 NOT NULL,
	LastDateOccupied DATETIME2 NOT NULL,
	TotalDays DATE NOT NULL,
	AmountCharged DECIMAL(6,2) NOT NULL,
	TaxRate DECIMAL(6,2) NOT NULL,
	TaxAmount DECIMAL(6,2) NOT NULL,
	PaymentTotal DECIMAL(8,2) NOT NULL,
	Notes VARCHAR(200)
)

INSERT INTO Payments(EmployeeId, PaymentDate, AccountNumber, FirstDateOccupied, LastDateOccupied, TotalDays, AmountCharged, TaxRate, TaxAmount, PaymentTotal, Notes) VALUES
(1, '2020-10-10', 1, '2020-10-5', '2020-10-10', '2020-10-10', 5, 0, 0, 5, NULL),
(2, '2020-11-10', 2, '2020-11-5', '2020-11-10', '2020-11-10', 5, 0, 0, 5, NULL),
(3, '2020-12-10', 3, '2020-12-5', '2020-12-10', '2020-12-10', 5, 0, 0, 5, NULL)

--Occupancies (Id, EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes)
CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	DateOccupied DATETIME2 NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber) NOT NULL,
	RateApplied DECIMAL(2,1),
	PhoneCharge DECIMAL(5,2) NOT NULL,
	Notes VARCHAR(200)
)

INSERT INTO Occupancies(EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge, Notes) VALUES
(1, '2020-10-10', 1, 1, 0, 0, NULL),
(2, '2020-11-10', 2, 2, 0, 0, NULL),
(3, '2020-12-10', 3, 3, 0, 0, NULL)

--P16--
CREATE DATABASE SoftUni
USE SoftUni

--Towns (Id, Name)
CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

INSERT INTO Towns([Name]) VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

--Addresses (Id, AddressText, TownId)
CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	AddressText VARCHAR(80) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL
)

--Departments (Id, Name)
CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

INSERT INTO Departments([Name]) VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

--Employees (Id, FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	MiddleName VARCHAR(30),
	LastName VARCHAR(30) NOT NULL,
	JobTitle VARCHAR(30) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL,
	HireDate DATE NOT NULL,
	Salary DECIMAL(6,2) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)
--P17-- CREATED A BACKUP FOR SOFTUNI DATABASE

--P18--
INSERT INTO Employees(FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId) VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '2013-02-01', 3500.00, NULL),
('Petar', 'Petrov', 'Petrov',	'Senior Engineer',	1,	'2004-03-02', 4000.00, NULL),
('Maria', 'Petrova', 'Ivanova',	'Intern', 5, '2016-08-28',	525.25, NULL),
('Georgi', 'Teziev', 'Ivanov',	'CEO',	2, '2007-12-09', 3000.00, NULL),
('Peter', 'Pan', 'Pan',	'Intern', 3, '2016-08-28', 599.88, NULL)

--P19--
SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees

--P20--
SELECT * FROM Towns
ORDER BY [Name] ASC

SELECT * FROM Departments
ORDER BY [Name] ASC

SELECT * FROM Employees
ORDER BY Salary DESC

--P21--
SELECT [Name] FROM Towns
ORDER BY [Name] ASC

SELECT [Name] FROM Departments
ORDER BY [Name] ASC

SELECT FirstName, LastName, JobTitle, Salary FROM Employees
ORDER BY Salary DESC

--P22--
UPDATE Employees SET Salary *= 1.10
SELECT Salary FROM Employees

--P23--
USE Hotel

UPDATE Payments SET TaxRate *= 0.97

SELECT TaxRate FROM Payments

--P24--
DELETE FROM Occupancies