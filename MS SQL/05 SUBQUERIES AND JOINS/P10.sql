SELECT TOP(50) E.EmployeeID, 
			   CONCAT(E.FirstName, ' ', E.LastName) AS EmployeeName, 
			   CONCAT(EMP.FirstName, ' ',  EMP.LastName) AS ManagerName, 
			   D.[Name] AS DepartmentName
	FROM Employees AS E
	JOIN Employees AS EMP ON E.ManagerID = EMP.EmployeeID
	JOIN Departments AS D ON E.DepartmentID = D.DepartmentID
	ORDER BY E.EmployeeID