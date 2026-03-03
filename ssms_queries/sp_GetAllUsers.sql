CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        FullName,
        Email,
        RoleId,       -- Use RoleId instead of Role
        IsActive,
        CreatedDate
    FROM Users
    WHERE IsActive = 1
END
