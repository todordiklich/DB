SELECT TOP(10) FirstName, LastName, E.DepartmentID
FROM Employees AS E
LEFT JOIN   (SELECT DepartmentID, AVG(Salary) AS AvgSalary
			FROM Employees
			GROUP BY DepartmentID) AS D ON D.DepartmentID = E.DepartmentID
WHERE E.Salary > D.AvgSalary