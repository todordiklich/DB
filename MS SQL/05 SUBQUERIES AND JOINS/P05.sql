SELECT TOP(3) E.EmployeeID, E.FirstName
FROM Employees AS E
LEFT JOIN EmployeesProjects AS P ON E.EmployeeID = P.EmployeeID
WHERE P.ProjectID IS NULL
ORDER BY E.EmployeeID