CREATE PROCEDURE sp_InsertMatch
(
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
    INSERT INTO Matches
    (
        MatchDate,
        GroundName,
        TeamA,
        TeamB,
        Overs,
        MatchType,
        WinnerTeam
    )
    VALUES
    (
        @MatchDate,
        @GroundName,
        @TeamA,
        @TeamB,
        @Overs,
        @MatchType,
        @WinnerTeam
    )
END
