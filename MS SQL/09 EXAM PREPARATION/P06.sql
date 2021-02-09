SELECT f.Id, SUM(t.Price)
FROM Flights AS f
JOIN Tickets AS t ON t.FlightId = f.Id
GROUP BY f.Id
ORDER BY SUM(t.Price) DESC, f.Id