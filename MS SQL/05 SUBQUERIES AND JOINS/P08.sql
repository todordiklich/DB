SELECT E.EmployeeID, 
	   E.FirstName,
	   CASE
	   WHEN DATEPART(YEAR, P.StartDate) >= 2005 THEN NULL
	   ELSE P.[Name]
	   END [ProjectName]
FROM Employees AS E
JOIN EmployeesProjects AS PE ON E.EmployeeID = PE.EmployeeID
JOIN Projects AS P ON PE.ProjectID = P.ProjectID
WHERE E.EmployeeID = 24