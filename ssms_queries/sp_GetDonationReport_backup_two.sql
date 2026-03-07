--ALTER PROCEDURE sp_GetDonationReport
--    @UserId INT = NULL  -- optional filter
--AS
--BEGIN
--    SET NOCOUNT ON;

--    SELECT   
--        d.DonationId,  
--        u.FullName,  
--        c.CelebrationName,  
--        d.Amount,  
--        d.Year,  
--        d.DonationDate  
--    FROM Donations d  
--    INNER JOIN Users u ON d.UserId = u.Id  
--    INNER JOIN Celebrations c ON d.CelebrationId = c.Id  
--    WHERE (@UserId IS NULL OR d.UserId = @UserId)  -- filter only if userId is passed
--    ORDER BY d.DonationDate DESC;
--END

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
    WHERE (@UserId IS NULL OR d.UserId = @UserId)  -- Admin: @UserId NULL → all, User: filter by UserId
    ORDER BY d.DonationDate DESC;
END
