SELECT t.Id,
	   a.FirstName + ' ' + IIF(a.MiddleName IS NULL, '', a.MiddleName + ' ') + a.LastName AS [Full Name],
	   c.Name,
	   c2.Name,
	   CASE
		WHEN CancelDate IS NOT NULL THEN 'Canceled'
		ELSE CONVERT(VARCHAR, DATEDIFF(DAY, ArrivalDate, ReturnDate)) + ' days'
	   END
FROM AccountsTrips AS atr
JOIN Accounts AS a ON atr.AccountId = a.Id
JOIN Trips AS t ON atr.TripId = t.Id
JOIN Cities AS c ON a.CityId = c.Id
JOIN Rooms AS r ON t.RoomId = r.Id
JOIN Hotels AS h ON r.HotelId = h.Id
JOIN Cities AS c2 ON h.CityId = c2.Id
ORDER BY [Full Name], t.Id