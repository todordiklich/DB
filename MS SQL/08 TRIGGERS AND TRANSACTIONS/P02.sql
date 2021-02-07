CREATE TRIGGER tr_notificationEmails
ON Logs FOR INSERT
AS
DECLARE @recipientId INT = (SELECT TOP(1) AccountId FROM inserted)
DECLARE @oldBalance DECIMAL(15,2) = (SELECT TOP(1) OldSum FROM inserted)
DECLARE @newBalance DECIMAL(15,2) = (SELECT TOP(1) NewSum FROM inserted)

INSERT INTO NotificationEmails(Recipient, [Subject], Body) VALUES
(@recipientId, 
'Balance change for account: ' + CONVERT(VARCHAR, @recipientId), 
'On ' + CONVERT(VARCHAR, GETDATE(), 109) + ' your balance was changed from ' +  CONVERT(VARCHAR, @oldBalance) +  ' to ' + CONVERT(VARCHAR, @newBalance))