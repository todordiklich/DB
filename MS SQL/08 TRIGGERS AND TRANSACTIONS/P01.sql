CREATE TRIGGER tr_AddLogsWhenChangeInAccounts
ON Accounts FOR UPDATE
AS
INSERT INTO Logs(AccountId, OldSum, NewSum)
SELECT i.Id, d.Balance, i.Balance
FROM inserted AS i
JOIN deleted AS d ON i.Id = d.Id
WHERE i.Balance != d.Balance