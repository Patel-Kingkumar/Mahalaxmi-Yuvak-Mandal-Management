CREATE TABLE Matches
(
    MatchId INT IDENTITY(1,1) PRIMARY KEY,
    MatchDate DATETIME,
    GroundName VARCHAR(200),
    TeamA VARCHAR(100),
    TeamB VARCHAR(100),
    Overs INT,
    MatchType VARCHAR(50), -- Friendly / Tournament
    WinnerTeam VARCHAR(100)
)
