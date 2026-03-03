CREATE TABLE LoginHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    LoginTime DATETIME DEFAULT GETDATE(),
    IpAddress NVARCHAR(100),
    IsSuccess BIT,

    CONSTRAINT FK_LoginHistory_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);