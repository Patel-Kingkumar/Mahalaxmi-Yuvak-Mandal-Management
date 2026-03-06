CREATE TABLE Donations (
    DonationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CelebrationId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Year INT NOT NULL,
    DonationDate DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Donations_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id),

    CONSTRAINT FK_Donations_Celebrations 
        FOREIGN KEY (CelebrationId) REFERENCES Celebrations(Id)
);


select * from Users