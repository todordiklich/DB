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
	Id INT NOT NULL IDENTITY PRIMARY KEY,
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
	Id BIGINT PRIMARY KEY IDENTITY NOT NULL,
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
DROP CONSTRAINT PK__Users__3214EC07816108A3

ALTER TABLE Users
ADD CONSTRAINT PK_IdUsername PRIMARY KEY(Id, Username)

--P10--
ALTER TABLE Users
ADD CONSTRAINT CH_Password CHECK([Password] >= 5)

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
ADD CONSTRAINT CH_Username CHECK (Username >= 3)