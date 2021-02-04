CREATE PROCEDURE usp_CalculateFutureValueForAccount (@AccountId INT, @InterestRate FLOAT)
AS
SELECT a.Id, FirstName, LastName, Balance AS [Current Balance], dbo.ufn_CalculateFutureValue(a.Balance, @InterestRate, 5) AS [Balance in 5 years]
FROM Accounts AS a
JOIN AccountHolders AS ah ON a.AccountHolderId = ah.Id
WHERE a.Id = @AccountId