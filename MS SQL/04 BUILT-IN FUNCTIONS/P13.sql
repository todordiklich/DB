SELECT Peaks.PeakName, Rivers.RiverName, LOWER(Peaks.PeakName + SUBSTRING(Rivers.RiverName, 2, LEN(Rivers.RiverName))) AS Mix
FROM Peaks, Rivers
WHERE RIGHT(Peaks.PeakName, 1) = LEFT(Rivers.RiverName, 1)
ORDER BY Mix