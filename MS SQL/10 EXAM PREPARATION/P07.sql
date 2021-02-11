SELECT a.Id, a.FirstName + ' ' + a.LastName AS FullName, MAX(DATEDIFF(DAY, ArrivalDate, ReturnDate)) AS LongestTrip, MIN(DATEDIFF(DAY, ArrivalDate, ReturnDate))  AS ShortestTrip
FROM Accounts AS a
JOIN AccountsTrips AS [at] ON [at].AccountId = a.Id
JOIN Trips AS t ON [at].TripId = t.Id
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY a.Id, a.FirstName + ' ' + a.LastName
ORDER BY LongestTrip DESC, ShortestTrip ASC