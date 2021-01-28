SELECT TOP(1) AVG(Salary) AS MinAverageSalary
FROM Employees AS E
JOIN Departments AS D ON E.DepartmentID = D.DepartmentID
GROUP BY D.[Name]
ORDER BY MinAverageSalary