SELECT a.Id, a.Email, c.Name, COUNT(*) AS TripsCount
FROM AccountsTrips AS atr
JOIN Accounts AS a ON atr.AccountId = a.Id
JOIN Trips AS t ON atr.TripId = t.Id
JOIN Cities AS c ON a.CityId = c.Id
JOIN Rooms AS r ON t.RoomId = r.Id
JOIN Hotels AS h ON r.HotelId = h.Id
WHERE a.CityId = h.CityId
GROUP BY a.Id, a.Email, c.Name
ORDER BY TripsCount DESC, a.Id