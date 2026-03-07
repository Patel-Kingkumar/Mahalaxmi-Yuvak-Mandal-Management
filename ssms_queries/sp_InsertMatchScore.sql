CREATE PROCEDURE sp_InsertMatchScore
(
    @MatchId INT,
    @TeamName VARCHAR(100),
    @Runs INT,
    @Wickets INT,
    @OversPlayed DECIMAL(4,1)
)
AS
BEGIN
    INSERT INTO MatchScores
    (
        MatchId,
        TeamName,
        Runs,
        Wickets,
        OversPlayed
    )
    VALUES
    (
        @MatchId,
        @TeamName,
        @Runs,
        @Wickets,
        @OversPlayed
    )
END
