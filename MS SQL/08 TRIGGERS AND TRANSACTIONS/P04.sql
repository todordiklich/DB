CREATE PROC usp_WithdrawMoney (@accountId INT, @moneyAmount DECIMAL(15,4))
AS
BEGIN TRANSACTION
DECLARE @checkAccountId INT = (SELECT TOP(1) Id FROM Accounts WHERE Id = @accountId)
DECLARE @accountBalance DECIMAL(15,2) = (SELECT Balance FROM Accounts WHERE Id= @accountId)
IF(@checkAccountId IS NULL)
BEGIN
	ROLLBACK
	RAISERROR  ('Ivalid account!', 16, 1)
	RETURN
END
IF (@moneyAmount < 0)
BEGIN
	ROLLBACK
	RAISERROR ('Invalid amount!', 16, 1)
	RETURN
END
IF (@accountBalance < @moneyAmount)
BEGIN
	ROLLBACK
	RAISERROR ('Insufficient funds!', 16, 1)
	RETURN
END

UPDATE Accounts
SET Balance -= @moneyAmount
WHERE Id = @accountId

COMMIT
