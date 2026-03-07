CREATE PROCEDURE sp_TotalMatches
AS
BEGIN
    SELECT COUNT(*) AS TotalMatches
    FROM Matches
END
