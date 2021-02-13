CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(30))
AS
SELECT Id, Name, CONVERT(VARCHAR, Size) + 'KB' AS Size
FROM Files
WHERE Name LIKE '%' + @fileExtension
ORDER BY Id, Name, Size DESC