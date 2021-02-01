SELECT SUM(RES.[Sum])
FROM (SELECT FirstName, DepositAmount - LEAD(DepositAmount, 1) OVER (ORDER BY Id) AS [Sum]
FROM WizzardDeposits) AS RES