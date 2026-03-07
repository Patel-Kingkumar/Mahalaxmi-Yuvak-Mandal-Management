CREATE PROCEDURE sp_UpdateMatch
(
    @MatchId INT,
    @MatchDate DATETIME,
    @GroundName VARCHAR(200),
    @TeamA VARCHAR(100),
    @TeamB VARCHAR(100),
    @Overs INT,
    @MatchType VARCHAR(50),
    @WinnerTeam VARCHAR(100)
)
AS
BEGIN
    UPDATE Matches
    SET
        MatchDate = @MatchDate,
        GroundName = @GroundName,
        TeamA = @TeamA,
        TeamB = @TeamB,
        Overs = @Overs,
        MatchType = @MatchType,
        WinnerTeam = @WinnerTeam
    WHERE MatchId = @MatchId
END
