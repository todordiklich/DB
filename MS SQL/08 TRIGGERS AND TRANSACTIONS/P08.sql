CREATE PROC usp_AssignProject (@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
DECLARE @projectsCount INT = (SELECT COUNT(*) 
							   FROM EmployeesProjects AS ep
							   JOIN Employees AS e ON ep.EmployeeID = e.EmployeeID
							   JOIN Projects AS p ON ep.ProjectID = p.ProjectID
							   WHERE e.EmployeeID = @emloyeeId)

IF (@projectsCount >= 3)
BEGIN
	ROLLBACK
	RAISERROR ('The employee has too many projects!', 16, 1)
	RETURN
END

INSERT INTO EmployeesProjects (EmployeeID, ProjectID) VALUES (@emloyeeId, @projectID)
COMMIT