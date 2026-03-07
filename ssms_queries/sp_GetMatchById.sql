CREATE PROCEDURE sp_GetMatchById
(
    @MatchId INT
)
AS
BEGIN
    SELECT * FROM Matches
    WHERE MatchId = @MatchId
END
