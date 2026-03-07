CREATE PROCEDURE sp_DeleteMatch
(
    @MatchId INT
)
AS
BEGIN
    DELETE FROM Matches
    WHERE MatchId = @MatchId
END
