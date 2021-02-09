SELECT FirstName + ' ' + LastName AS [Full Name], Origin, Destination
FROM Passengers AS p
JOIN Tickets AS t ON p.Id = t.PassengerId
JOIN Flights AS f ON t.FlightId = f.Id
ORDER BY [Full Name], Origin, Destination