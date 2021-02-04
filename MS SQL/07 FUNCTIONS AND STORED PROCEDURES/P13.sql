CREATE FUNCTION ufn_CashInUsersGames (@GameName VARCHAR(100))
RETURNS TABLE
AS 
RETURN (SELECT SUM(Cash) AS SumCash
			FROM (SELECT Cash, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS [Row]
			FROM UsersGames AS ug
			JOIN Games as g ON ug.GameId = g.Id
			WHERE g.[Name] = @GameName) AS k
		WHERE k.[Row] % 2 = 1)