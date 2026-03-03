ALTER PROCEDURE sp_CreateUser
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(MAX),
    @RoleId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (FullName, Email, PasswordHash, RoleId, IsActive, CreatedDate)
    VALUES (@FullName, @Email, @PasswordHash, @RoleId, @IsActive, GETDATE());
END