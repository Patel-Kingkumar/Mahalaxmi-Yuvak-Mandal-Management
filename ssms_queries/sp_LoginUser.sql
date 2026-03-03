ALTER PROCEDURE sp_LoginUser
    @Email NVARCHAR(150)
AS
BEGIN
    SELECT U.Id,
           U.FullName,
           U.Email,
           U.PasswordHash,
           R.RoleName
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id
    WHERE U.Email = @Email
      AND U.IsActive = 1
END
