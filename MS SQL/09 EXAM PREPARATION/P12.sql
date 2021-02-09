CREATE PROC usp_CancelFlights
AS

UPDATE Flights
SET ArrivalTime = NULL, DepartureTime = NULL
WHERE ArrivalTime > DepartureTime