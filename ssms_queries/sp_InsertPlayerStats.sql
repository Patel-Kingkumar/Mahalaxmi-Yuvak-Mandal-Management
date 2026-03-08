ALTER PROCEDURE sp_InsertPlayerStats
(
    @MatchId INT,
    @PlayerName VARCHAR(200),
    @TeamName VARCHAR(100),
    @Runs INT,
    @BallsFaced INT,
    @Fours INT,
    @Sixes INT,
    @OversBowled DECIMAL(5,2),
    @RunsConceded INT,
    @Wickets INT
)
AS
BEGIN

INSERT INTO PlayerStats
(
    MatchId,
    PlayerName,
    TeamName,
    Runs,
    BallsFaced,
    Fours,
    Sixes,
    OversBowled,
    RunsConceded,
    Wickets
)
VALUES
(
    @MatchId,
    @PlayerName,
    @TeamName,
    @Runs,
    @BallsFaced,
    @Fours,
    @Sixes,
    @OversBowled,
    @RunsConceded,
    @Wickets
)

END
