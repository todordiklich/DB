SELECT TOP(5) r.Id, r.Name, COUNT(*)
FROM RepositoriesContributors AS rc
JOIN Repositories AS r ON rc.RepositoryId = r.Id
JOIN Commits AS c ON r.Id = c.RepositoryId
GROUP BY r.Id, r.Name
ORDER BY COUNT(*) DESC, r.Id, r.Name