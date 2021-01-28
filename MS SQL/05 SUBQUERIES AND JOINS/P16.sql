SELECT COUNT(C.CountryCode) AS [Count]
FROM Countries AS C
LEFT JOIN MountainsCountries AS MC ON C.CountryCode = MC.CountryCode
LEFT JOIN Mountains AS M ON M.Id = MC.MountainId
WHERE MC.MountainId IS NULL