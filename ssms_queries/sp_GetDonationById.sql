CREATE PROCEDURE sp_GetDonationById
    @DonationId INT
AS
BEGIN
    SELECT
        d.DonationId,
        u.Id AS UserId,
        u.FullName,
        c.Id AS CelebrationId,
        c.CelebrationName,
        d.Amount,
        d.Year,
        d.DonationDate
    FROM Donations d
    INNER JOIN Users u 
        ON d.UserId = u.Id
    INNER JOIN Celebrations c 
        ON d.CelebrationId = c.Id
    WHERE d.DonationId = @DonationId
END
