SELECT FirstName + ' ' + LastName AS [Full Name], pl.[Name], f.Origin + ' - ' + f.Destination AS Trip, lt.[Type]
FROM Passengers AS p
JOIN Tickets AS t ON t.PassengerId = p.Id
JOIN Flights AS f ON t.FlightId = f.Id
JOIN Planes AS pl ON f.PlaneId = pl.Id
JOIN Luggages AS l ON l.Id = t.LuggageId
JOIN LuggageTypes AS lt ON l.LuggageTypeId = lt.Id
ORDER BY [Full Name], pl.[Name], f.Origin, f.Destination, lt.[Type]