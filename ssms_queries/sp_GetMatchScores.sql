CREATE PROCEDURE sp_GetMatchScores
(
    @MatchId INT
)
AS
BEGIN
    SELECT *
    FROM MatchScores
    WHERE MatchId = @MatchId
END
