SELECT p.Name, Seats, COUNT(pn.Id)
FROM Planes AS p
LEFT JOIN Flights AS f ON f.PlaneId = p.Id
LEFT JOIN Tickets AS t ON t.FlightId = f.Id
LEFT JOIN Passengers AS pn ON t.PassengerId = pn.Id
GROUP BY p.Id, p.Name, p.Seats
ORDER BY COUNT(pn.Id) DESC, p.Name, p.Seats