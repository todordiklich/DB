SELECT TOP(5) R.CountryName,
	   CASE
		WHEN R.PeakName IS NULL THEN '(no highest peak)'
		ELSE R.PeakName
	   END AS [Highest Peak Name],
	   CASE
		WHEN R.Elevation IS NULL THEN '0'
		ELSE R.Elevation
	   END AS [Highest Peak Elevation],
	   CASE
		WHEN R.MountainRange IS NULL THEN '(no mountain)'
		ELSE R.MountainRange
	   END AS [Mountain]
FROM 
	(
	  SELECT C.CountryName, 
	     P.PeakName, 
	     P.Elevation, 
	     M.MountainRange, 
	     DENSE_RANK() OVER(PARTITION BY C.CountryName ORDER BY P.Elevation DESC) AS [Rank]
	  FROM Countries AS C
	  LEFT JOIN MountainsCountries AS MC ON C.CountryCode = MC.CountryCode
	  LEFT JOIN Mountains AS M ON M.Id = MC.MountainId
	  LEFT JOIN Peaks AS P ON P.MountainId = M.Id
	) AS R
WHERE R.[Rank] = 1
ORDER BY R.CountryName, [Highest Peak Name]