UPDATE Tickets 
SET Price *= 1.13
WHERE FlightId IN (SELECT FlightId FROM Tickets AS t JOIN Flights AS f ON t.FlightId = f.Id WHERE Destination = 'Carlsbad')