CREATE PROC usp_GetTownsStartingWith(@Letter VARCHAR(50))
AS
SELECT [Name]
FROM Towns
WHERE [Name] LIKE @Letter + '%'