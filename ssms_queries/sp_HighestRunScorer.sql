CREATE PROCEDURE sp_HighestRunScorer
AS
BEGIN
    SELECT TOP 1
        PlayerName,
        SUM(RunsScored) AS TotalRuns
    FROM PlayerStats
    GROUP BY PlayerName
    ORDER BY TotalRuns DESC
END
