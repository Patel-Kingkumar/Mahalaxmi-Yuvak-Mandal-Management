CREATE TABLE PlayerStats
(
    PlayerStatId INT IDENTITY(1,1) PRIMARY KEY,

    MatchId INT NOT NULL,

    UserId INT NOT NULL,

    TeamName VARCHAR(100) NOT NULL,

    RunsScored INT DEFAULT 0,

    BallsPlayed INT DEFAULT 0,

    Fours INT DEFAULT 0,

    Sixes INT DEFAULT 0,

    WicketsTaken INT DEFAULT 0,

    OversBowled DECIMAL(4,1) DEFAULT 0,

    RunsGiven INT DEFAULT 0,

    Catches INT DEFAULT 0,

    CONSTRAINT FK_PlayerStats_Matches
        FOREIGN KEY (MatchId) REFERENCES Matches(MatchId),

    CONSTRAINT FK_PlayerStats_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
)
