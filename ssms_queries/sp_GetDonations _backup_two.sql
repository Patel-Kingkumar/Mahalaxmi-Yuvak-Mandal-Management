ALTER PROCEDURE sp_GetDonations  
    @UserId INT = NULL  
AS  
BEGIN  
    SELECT   
        d.DonationId,  
        d.UserId,  
        u.FullName,  
        c.CelebrationName,  
        d.Amount,  
        d.Year,  
        d.DonationDate  
    FROM Donations d  
    INNER JOIN Users u ON d.UserId = u.Id  
    INNER JOIN Celebrations c ON d.CelebrationId = c.Id  
    WHERE   
        (@UserId IS NULL OR d.UserId = @UserId)  -- Admin vs User
    ORDER BY d.DonationDate DESC  
END