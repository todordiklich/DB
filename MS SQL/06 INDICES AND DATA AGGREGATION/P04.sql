SELECT TOP(2) RES.DepositGroup
FROM (SELECT DepositGroup, AVG(MagicWandSize) AS AvgWand
FROM WizzardDeposits
GROUP BY DepositGroup) AS Res
ORDER BY RES.AvgWand