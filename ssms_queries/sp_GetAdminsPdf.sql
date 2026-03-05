CREATE PROCEDURE sp_GetAdminsPdf
AS
BEGIN
    SELECT 
        u.Id,
        u.FullName,
        u.Email,
        r.RoleName AS Role,
        u.IsActive,
        u.CreatedDate
    FROM Users u
    INNER JOIN Roles r ON u.RoleId = r.Id
    WHERE r.RoleName = 'Admin'
END
