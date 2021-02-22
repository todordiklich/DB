SELECT TOP(10) c.Id, c.Name, c.CountryCode, COUNT(*) AS AccountsNumber
FROM Cities AS c
JOIN Accounts AS a ON a.CityId = c.Id
GROUP BY c.Id, c.Name, c.CountryCode
ORDER BY AccountsNumber DESC