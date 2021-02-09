CREATE FUNCTION udf_CalculateTickets(@origin VARCHAR(50), @destination VARCHAR(50), @peopleCount INT)
RETURNS VARCHAR(100)
AS
BEGIN
IF (@peopleCount <= 0)
RETURN 'Invalid people count!'

DECLARE @doesOriginExist VARCHAR(50) = (SELECT TOP(1) Origin FROM Flights WHERE Origin = @origin)
DECLARE @doesDestinationExist VARCHAR(50) = (SELECT TOP(1) Destination FROM Flights WHERE Destination = @destination)

IF (@doesOriginExist IS NULL OR @doesDestinationExist IS NULL)
RETURN 'Invalid flight!'

DECLARE @singlePrice DECIMAL(15,2) = (SELECT Price
									  FROM Flights AS f
									  JOIN Tickets AS t ON t.FlightId = f.Id
									  WHERE Origin = @origin AND Destination = @destination)

DECLARE @totalPrice DECIMAL(15,2) = @singlePrice * @peopleCount
RETURN 'Total price ' + CONVERT(VARCHAR, @totalPrice)
END