CREATE TABLE MatchScores
(
    ScoreId INT IDENTITY(1,1) PRIMARY KEY,
    MatchId INT,
    TeamName VARCHAR(100),
    Runs INT,
    Wickets INT,
    OversPlayed DECIMAL(4,1),

    FOREIGN KEY (MatchId) REFERENCES Matches(MatchId)
)
