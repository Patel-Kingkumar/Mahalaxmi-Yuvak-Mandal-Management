CREATE PROCEDURE sp_GetPlayerPerformance
AS
BEGIN
    SELECT 
        PlayerName,
        Runs,
        Wickets
    FROM PlayerStats
END
