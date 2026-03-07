CREATE PROCEDURE sp_InsertPlayerStats
(
    @MatchId INT,
    @PlayerName VARCHAR(200),
    @TeamName VARCHAR(100),
    @RunsScored INT,
    @BallsPlayed INT,
    @Fours INT,
    @Sixes INT,
    @WicketsTaken INT,
    @OversBowled DECIMAL(4,1),
    @RunsGiven INT,
    @Catches INT
)
AS
BEGIN
    INSERT INTO PlayerStats
    (
        MatchId,
        PlayerName,
        TeamName,
        RunsScored,
        BallsPlayed,
        Fours,
        Sixes,
        WicketsTaken,
        OversBowled,
        RunsGiven,
        Catches
    )
    VALUES
    (
        @MatchId,
        @PlayerName,
        @TeamName,
        @RunsScored,
        @BallsPlayed,
        @Fours,
        @Sixes,
        @WicketsTaken,
        @OversBowled,
        @RunsGiven,
        @Catches
    )
END
