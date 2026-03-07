CREATE PROCEDURE sp_MostWickets
AS
BEGIN
    SELECT TOP 1
        PlayerName,
        SUM(WicketsTaken) AS TotalWickets
    FROM PlayerStats
    GROUP BY PlayerName
    ORDER BY TotalWickets DESC
END
