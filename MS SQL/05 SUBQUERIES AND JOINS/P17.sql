SELECT TOP(5) R.CountryName,
	   MAX(R.HighestPeakElevation) AS HighestPeakElevation,
	   MAX(R.LongestRiverLength) AS LongestRiverLength
FROM
	(
	SELECT C.CountryName , P.Elevation AS HighestPeakElevation, R.Length AS LongestRiverLength
	FROM Countries AS C
	JOIN MountainsCountries AS MC ON C.CountryCode = MC.CountryCode
	JOIN Mountains AS M ON M.Id = MC.MountainId
	JOIN Peaks AS P ON P.MountainId = M.Id
	JOIN CountriesRivers AS CR ON C.CountryCode = CR.CountryCode
	JOIN Rivers AS R ON CR.RiverId = R.Id
	) AS R
GROUP BY R.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, R.CountryName	