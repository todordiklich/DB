CREATE FUNCTION ufn_CalculateFutureValue (
							@Sum DECIMAL(15,2), 
							@YearlyInterestRate FLOAT, 
							@Years INT)
RETURNS DECIMAL(15,4)
AS BEGIN
	DECLARE @Result DECIMAL(15,4)
	SET @Result = (@Sum * (POWER((1 + @YearlyInterestRate), @Years)))
	RETURN @Result
END