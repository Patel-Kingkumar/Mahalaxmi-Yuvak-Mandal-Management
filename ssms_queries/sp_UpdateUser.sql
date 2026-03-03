CREATE PROCEDURE sp_UpdateUser
    @Id INT,
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @RoleId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET 
        FullName = @FullName,
        Email = @Email,
        RoleId = @RoleId,
        IsActive = @IsActive
    WHERE Id = @Id
END