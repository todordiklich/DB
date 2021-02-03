CREATE PROC usp_EmployeesBySalaryLevel(@SalaryLevel NVARCHAR(100))
AS
SELECT FirstName, LastName
FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel