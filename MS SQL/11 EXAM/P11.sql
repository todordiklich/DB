CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(30))
RETURNS INT
AS
BEGIN
DECLARE @userID INT = (SELECT Id FROM Users WHERE Username = @username)

IF (@userID IS NULL)
	RETURN 0

RETURN (SELECT COUNT(*)
FROM Users u
JOIN Commits c ON c.ContributorId = u.Id
WHERE u.Id = @userID)

END