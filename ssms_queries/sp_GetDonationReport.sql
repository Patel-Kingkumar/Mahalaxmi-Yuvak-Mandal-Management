ALTER PROCEDURE sp_GetDonationReport  
    @UserId INT = NULL  -- optional filter  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    SELECT     
        d.DonationId,    
        u.FullName,    
        c.CelebrationName,    
        d.Amount,    
        d.Year,    
        d.DonationDate    
    FROM Donations d    
    INNER JOIN Users u ON d.UserId = u.Id    
    INNER JOIN Celebrations c ON d.CelebrationId = c.Id    
    WHERE (@UserId IS NULL OR d.UserId = @UserId)  -- filter only if userId is passed  
    ORDER BY d.DonationDate DESC;  
END  
  
ALTER PROCEDURE sp_GetDonationReport
    @UserId INT,      -- logged-in user ID
    @RoleId INT       -- logged-in user role ID
AS  
BEGIN  
    SET NOCOUNT ON;  

    SELECT     
        d.DonationId,    
        u.FullName,    
        r.RoleName AS Role,  
        c.CelebrationName,    
        d.Amount,    
        d.Year,    
        d.DonationDate    
    FROM Donations d    
    INNER JOIN Users u ON d.UserId = u.Id    
    INNER JOIN Roles r ON u.RoleId = r.Id
    INNER JOIN Celebrations c ON d.CelebrationId = c.Id    
    WHERE (@RoleId = 1 OR d.UserId = @UserId)   -- Admin sees all, else only own
    ORDER BY d.DonationDate DESC;  
END