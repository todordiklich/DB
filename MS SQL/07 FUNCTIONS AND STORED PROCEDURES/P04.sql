CREATE PROC usp_GetEmployeesFromTown (@TownaName NVARCHAR(100))
AS
SELECT FirstName, LastName
FROM Employees AS e
JOIN Addresses AS a ON e.AddressID = a.AddressID
JOIN Towns AS t ON t.TownID = a.TownID
WHERE t.[Name] = @TownaName