CREATE TABLE PlayerStats
(
    PlayerStatId INT IDENTITY(1,1) PRIMARY KEY,
    MatchId INT,
    PlayerName VARCHAR(200),
    TeamName VARCHAR(100),
    RunsScored INT,
    BallsPlayed INT,
    Fours INT,
    Sixes INT,
    WicketsTaken INT,
    OversBowled DECIMAL(4,1),
    RunsGiven INT,
    Catches INT,

    FOREIGN KEY (MatchId) REFERENCES Matches(MatchId)
)
