---- SP → User Wise Donations
--CREATE PROCEDURE sp_GetUserDonations
--(
--    @UserId INT
--)
--AS
--BEGIN

--SELECT 
--    u.FullName,
--    c.CelebrationName,
--    d.DonationAmount,
--    d.DonationYear,
--    d.DonationDate
--FROM Donations d
--INNER JOIN Users u ON u.Id = d.UserId
--INNER JOIN Celebrations c ON c.Id = d.CelebrationId
--WHERE d.UserId = @UserId
--ORDER BY d.DonationYear DESC

--END


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
        (@UserId IS NULL OR d.UserId = @UserId)
    ORDER BY d.DonationDate DESC
END
