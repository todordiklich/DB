CREATE PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS

DECLARE @tripHotel VARCHAR(100) = (SELECT TOP(1) h.Name FROM Trips AS t
											JOIN Rooms AS r ON t.RoomId = r.Id
											JOIN Hotels AS h ON r.HotelId = h.Id
											WHERE t.Id = @TripId)

DECLARE @targetRoomHotel VARCHAR(100) = (SELECT TOP(1) h.Name FROM Trips AS t
											JOIN Rooms AS r ON t.RoomId = r.Id
											JOIN Hotels AS h ON r.HotelId = h.Id
											WHERE r.Id = @TargetRoomId)

IF (@targetRoomHotel != @tripHotel)
	THROW 50001, 'Target room is in another hotel!', 1

DECLARE @bedsNeeded INT =  (SELECT COUNT(*) FROM AccountsTrips AS at JOIN Accounts AS a ON at.AccountId = a.Id WHERE at.TripId = @TripId)
DECLARE @bedsInRoom INT =  (SELECT Beds FROM Rooms AS r JOIN Hotels AS h ON r.HotelId = h.Id WHERE r.Id = @TargetRoomId)

IF (@bedsInRoom < @bedsNeeded)
	THROW 50001, 'Not enough beds in target room!', 1

UPDATE Trips
SET RoomId = @TargetRoomId
WHERE Id = @TripId