SELECT * INTO NewEmployees FROM Employees WHERE Salary > 30000

DELETE FROM NewEmployees WHERE ManagerID = 42

UPDATE NewEmployees
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary)
FROM NewEmployees
GROUP BY DepartmentID