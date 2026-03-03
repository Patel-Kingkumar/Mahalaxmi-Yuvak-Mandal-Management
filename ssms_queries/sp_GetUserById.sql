CREATE PROCEDURE sp_GetUserById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Id,
        U.FullName,
        U.Email,
        R.RoleName AS Role,
        U.PasswordHash,
        U.Otp,
        U.OtpExpiry,
        U.IsActive,
        U.CreatedDate
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id
    WHERE U.Id = @Id
END