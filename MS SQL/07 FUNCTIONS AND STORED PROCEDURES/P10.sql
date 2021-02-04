CREATE PROC usp_GetHoldersWithBalanceHigherThan(@salary DECIMAL(14,2))
AS
SELECT FirstName, LastName
	  FROM AccountHolders AS ah
	  JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	  GROUP BY FirstName, LastName
	  HAVING SUM(Balance) > @salary
	  ORDER BY FirstName, LastName