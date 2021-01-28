SELECT TOP(5) E.EmployeeID, E.FirstName, E.Salary, D.[Name] AS [DepartmentName]
FROM Employees AS E
JOIN Departments AS D ON D.DepartmentID = E.DepartmentID
WHERE E.Salary > 15000
ORDER BY D.DepartmentID