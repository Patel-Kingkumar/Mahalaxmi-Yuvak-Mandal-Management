CREATE PROCEDURE sp_GetDonations
    @Year INT = NULL,
    @UserId INT = NULL
AS
BEGIN
    SELECT 
        d.DonationId,
        d.UserId,
        u.FullName,
        d.CelebrationId,
        c.CelebrationName,
        d.Amount,
        d.Year,
        d.DonationDate
    FROM Donations d
    INNER JOIN Users u ON d.UserId = u.Id
    INNER JOIN Celebrations c ON d.CelebrationId = c.Id
    WHERE 
        (@Year IS NULL OR d.Year = @Year)
        AND (@UserId IS NULL OR d.UserId = @UserId)
    ORDER BY d.Year DESC, d.DonationDate DESC
END
