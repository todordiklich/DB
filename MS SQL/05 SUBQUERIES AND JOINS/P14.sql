SELECT TOP(5) C.CountryName, R.RiverName
FROM Countries AS C
LEFT JOIN CountriesRivers AS CR ON C.CountryCode = CR.CountryCode
LEFT JOIN Rivers AS R ON R.Id = CR.RiverId
WHERE C.ContinentCode = 'AF'
ORDER BY C.CountryName