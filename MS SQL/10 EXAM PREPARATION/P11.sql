CREATE FUNCTION udf_GetAvailableRoom (@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(MAX)
AS
BEGIN

DECLARE @roomPrice DECIMAL(15,2) = (SELECT TOP(1) Price
									FROM Rooms AS r
									JOIN Trips AS t ON t.RoomId = r.Id
									WHERE (HotelId = @HotelId) AND (Beds >= @People) AND NOT EXISTS(SELECT * FROM Trips tr WHERE tr.RoomId = r.Id AND CancelDate IS NULL AND @Date BETWEEN ArrivalDate AND ReturnDate)
									ORDER BY Price DESC)
IF(@roomPrice IS NULL)
RETURN 'No rooms available'

DECLARE @roomId INT = (SELECT TOP(1) r.Id
									FROM Rooms AS r
									JOIN Trips AS t ON t.RoomId = r.Id
									WHERE (HotelId = @HotelId) AND (Beds >= @People) AND ((@Date NOT BETWEEN ArrivalDate AND ReturnDate) OR (@Date BETWEEN ArrivalDate AND ReturnDate AND CancelDate IS NOT NULL))
									ORDER BY Price DESC)

DECLARE @roomType VARCHAR(100) = (SELECT TOP(1) Type
									FROM Rooms AS r
									JOIN Trips AS t ON t.RoomId = r.Id
									WHERE (HotelId = @HotelId) AND (Beds >= @People) AND ((@Date NOT BETWEEN ArrivalDate AND ReturnDate) OR (@Date BETWEEN ArrivalDate AND ReturnDate AND CancelDate IS NOT NULL))
									ORDER BY Price DESC)

DECLARE @roomBeds INT = (SELECT TOP(1) Beds
									FROM Rooms AS r
									JOIN Trips AS t ON t.RoomId = r.Id
									WHERE (HotelId = @HotelId) AND (Beds >= @People) AND ((@Date NOT BETWEEN ArrivalDate AND ReturnDate) OR (@Date BETWEEN ArrivalDate AND ReturnDate AND CancelDate IS NOT NULL))
									ORDER BY Price DESC)

DECLARE @hotelBaseRate DECIMAL(15,2) = (SELECT BaseRate FROM Hotels WHERE Id = @HotelId)
DECLARE @totalPrice DECIMAL(15,2) = ((@hotelBaseRate + @roomPrice) * @People)

RETURN 'Room '+ CONVERT(VARCHAR, @roomId) + ': ' + @roomType + ' (' + CONVERT(VARCHAR, @roomBeds) + ' beds) - $' + CONVERT(VARCHAR, @totalPrice)

END