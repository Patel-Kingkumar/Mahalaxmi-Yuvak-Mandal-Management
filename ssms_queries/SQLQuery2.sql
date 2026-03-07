ALTER PROCEDURE sp_GetDonationReport
AS
BEGIN
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
END