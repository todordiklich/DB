--There are two groups of items that you must buy for the above users. The first are items with id between 251 and 299 including. 
--Second group are items with id between 501 and 539 including.
--Take off cash from each user for the bought items.

SELECT *
FROM UserGameItems AS ugi
JOIN UsersGames AS ug ON ugi.UserGameId = ug.Id
JOIN Users AS u ON u.Id = ug.UserId
JOIN Games AS g ON ug.GameId = g.Id
--WHERE  g.[Name] = 'Bali' AND u.Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')
order by ug.Id

-- usersId: 61, 52, 37, 22, 12
-- gameId: 212

SELECT *
FROM Items
WHERE Id BETWEEN 251 AND 299

SELECT *
FROM UserGameItems
WHERE UserGameId IN (61, 52, 37, 22, 12)
ORDER BY UserGameId

ALTER PROC usp_BuyItems (@userId INT, @startItemId INT, @endtItemId INT)
AS
DECLARE @userCash DECIMAL(15,2) = (SELECT TOP(1) Cash FROM UsersGames WHERE UserId = @userId AND GameId = 212)
WHILE (@startItemId <= @endtItemId)
BEGIN
	DECLARE @itemPrice DECIMAL(15,2) = (SELECT Price FROM Items WHERE Id = @startItemId)
	IF @itemPrice <= @userCash
	BEGIN
	INSERT INTO UserGameItems (ItemId, UserGameId) VALUES(@startItemId, @userId)
	SET @startItemId += 1
	SET @userCash -= @itemPrice
	END
END
UPDATE UsersGames
SET Cash = @userCash
WHERE GameId = @userId AND GameId = 212

EXEC usp_BuyItems 61, 251, 299
EXEC usp_BuyItems 61, 501, 539

EXEC usp_BuyItems 52, 251, 299
EXEC usp_BuyItems 52, 501, 539

EXEC usp_BuyItems 37, 251, 299
EXEC usp_BuyItems 37, 501, 539

EXEC usp_BuyItems 22, 251, 299
EXEC usp_BuyItems 22, 501, 539

EXEC usp_BuyItems 12, 251, 299
EXEC usp_BuyItems 12, 501, 539

SELECT *
FROM UsersGames
WHERE UserId = 52