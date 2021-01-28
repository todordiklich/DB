SELECT R.ContinentCode, R.CurrencyCode, R.Total AS CurrencyUsage
FROM (
	   SELECT C.ContinentCode, 
			  CR.CurrencyCode, 
			  COUNT(CR.CurrencyCode) AS Total, 
			  DENSE_RANK() OVER (PARTITION BY C.ContinentCode ORDER BY COUNT(CR.CurrencyCode) DESC) AS Ranked
	   FROM Continents AS C
	   JOIN Countries AS CN ON C.ContinentCode = CN.ContinentCode
	   JOIN Currencies AS CR ON CN.CurrencyCode = CR.CurrencyCode
	   GROUP BY C.ContinentCode, CR.CurrencyCode
	  ) AS R
WHERE R.Total > 1 AND Ranked = 1
ORDER BY R.ContinentCode, R.CurrencyCode