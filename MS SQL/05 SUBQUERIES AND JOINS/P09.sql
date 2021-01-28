SELECT E.EmployeeID, E.FirstName, E.ManagerID, EMP.FirstName AS ManagerName
FROM Employees AS E
JOIN Employees AS EMP ON E.ManagerID = EMP.EmployeeID
WHERE E.ManagerID IN (3,7)
ORDER BY E.EmployeeID