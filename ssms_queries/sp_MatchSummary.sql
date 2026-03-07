CREATE PROCEDURE sp_MatchSummary
(
    @MatchId INT
)
AS
BEGIN
    SELECT *
    FROM Matches
    WHERE MatchId = @MatchId

    SELECT *
    FROM MatchScores
    WHERE MatchId = @MatchId
END
