ALTER PROCEDURE sp_GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.Id,
        U.FullName,
        U.Email,
        R.RoleName AS Role,   -- map RoleName to User.Role
        U.IsActive,
        U.CreatedDate
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id
    WHERE U.IsActive = 1
END