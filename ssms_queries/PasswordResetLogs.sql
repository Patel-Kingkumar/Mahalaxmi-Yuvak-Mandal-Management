CREATE TABLE PasswordResetLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    Otp NVARCHAR(10),
    GeneratedTime DATETIME DEFAULT GETDATE(),
    IsUsed BIT DEFAULT 0,

    CONSTRAINT FK_PasswordResetLogs_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);