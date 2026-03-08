CREATE TABLE PlayerStats
(
    PlayerStatId INT IDENTITY(1,1) PRIMARY KEY,
    MatchId INT,
    PlayerName VARCHAR(100),
    TeamName VARCHAR(100),
    Runs INT,
    BallsFaced INT,
    Fours INT,
    Sixes INT,
    OversBowled DECIMAL(4,1),
    RunsConceded INT,
    Wickets INT
)
