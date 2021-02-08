DECLARE @userGameId INT = (SELECT Id FROM UsersGames WHERE UserId = 9 AND GameId = 87)

DECLARE @cash DECIMAL(15,2) = (SELECT Cash 
							   FROM UsersGames AS ug
							   JOIN Users AS u ON ug.UserId = u.Id
							   JOIN Games AS g ON ug.GameId = g.Id
							   WHERE u.FirstName = 'Stamat' AND g.[Name] = 'Safflower')

DECLARE @neededCash DECIMAL(15,2) = (SELECT SUM(Price) AS TotalPrice
									 FROM Items
									 WHERE MinLevel BETWEEN 11 AND 12)

IF (@neededCash <= @cash)
BEGIN
BEGIN TRANSACTION
UPDATE UsersGames
SET Cash -= @neededCash
WHERE GameId = 87

INSERT INTO UserGameItems (ItemId, UserGameId)
SELECT Id, @userGameId FROM Items WHERE MinLevel BETWEEN 11 AND 12
COMMIT
END

SET @cash = (SELECT Cash 
							   FROM UsersGames AS ug
							   JOIN Users AS u ON ug.UserId = u.Id
							   JOIN Games AS g ON ug.GameId = g.Id
							   WHERE u.FirstName = 'Stamat' AND g.[Name] = 'Safflower')

DECLARE @neededCash1 DECIMAL(15,2) = (SELECT SUM(Price) AS TotalPrice
									 FROM Items
									 WHERE MinLevel BETWEEN 19 AND 21)

IF (@neededCash1 <= @cash)
BEGIN
BEGIN TRANSACTION
UPDATE UsersGames
SET Cash -= @neededCash1
WHERE GameId = 87

INSERT INTO UserGameItems (ItemId, UserGameId)
SELECT Id, @userGameId FROM Items WHERE MinLevel BETWEEN 19 AND 21
COMMIT
END

SELECT i.[Name] 
FROM UsersGames AS ug
JOIN Users AS u ON ug.UserId = u.Id
JOIN Games AS g ON ug.GameId = g.Id
JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id
JOIN Items AS i ON ugi.ItemId = i.Id
WHERE u.FirstName = 'Stamat' AND g.[Name] = 'Safflower'
ORDER BY i.[Name]