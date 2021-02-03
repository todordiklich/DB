CREATE FUNCTION ufn_IsWordComprised (@setOfLetters NVARCHAR(100), @word NVARCHAR(100))
RETURNS BIT
AS BEGIN
	DECLARE @count INT = 1
	WHILE (@count <= LEN(@word))
		BEGIN
			IF CHARINDEX(SUBSTRING(@word, @count, 1), @setOfLetters, 1) = 0
				RETURN 0
			SET @count += 1
		END
	RETURN 1
END