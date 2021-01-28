SELECT C.CountryCode, COUNT(M.MountainRange)
FROM Countries AS C
JOIN MountainsCountries AS MC ON C.CountryCode = MC.CountryCode
JOIN Mountains AS M ON MC.MountainId = M.Id
WHERE C.CountryCode IN ('BG', 'US', 'RU')
GROUP BY C.CountryCode