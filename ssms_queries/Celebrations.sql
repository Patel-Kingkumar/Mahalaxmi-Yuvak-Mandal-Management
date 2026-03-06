CREATE TABLE Celebrations
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CelebrationName NVARCHAR(150) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
)


INSERT INTO Celebrations (CelebrationName)
VALUES
('Mahalaxmi Mataji Salgiri'),
('Janmashtami'),
('Ganesh Mahotsav'),
('Navratri Mahotsav'),
('Dahanu Padyatra')
