CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN

-- Total Matches
SELECT COUNT(*) AS TotalMatches
FROM Matches;

-- Highest Run Scorer
SELECT TOP 1 
    PlayerName,
    SUM(Runs) AS TotalRuns
FROM PlayerStats
GROUP BY PlayerName
ORDER BY TotalRuns DESC;

-- Most Wickets
SELECT TOP 1 
    PlayerName,
    SUM(Wickets) AS TotalWickets
FROM PlayerStats
GROUP BY PlayerName
ORDER BY TotalWickets DESC;

END
