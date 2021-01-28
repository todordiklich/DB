SELECT E.FirstName, E.LastName, E.HireDate, D.[Name] AS [DeptName]
FROM Employees AS E
JOIN Departments AS D ON D.DepartmentID = E.DepartmentID
WHERE E.HireDate > '1999-01-01' AND D.Name IN ('Sales', 'Finance')
ORDER BY E.HireDate