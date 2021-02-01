SELECT DISTINCT DepartmentID, Salary AS ThirdHighestSalary
FROM(SELECT DepartmentID, Salary, DENSE_RANK() OVER(PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Rank]
FROM Employees) AS RES
WHERE RES.[Rank] = 3