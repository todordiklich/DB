SELECT Username, AVG(f.Size) AS Size
FROM Commits c
JOIN Users u ON c.ContributorId = u.Id
JOIN Files f ON f.CommitId = c.Id
GROUP BY Username
ORDER BY AVG(f.Size) DESC, Username