CREATE PROCEDURE sp_GetDonationsByYear
(
    @Year INT
)
AS
BEGIN

SELECT 
    u.FullName,
    c.CelebrationName,
    d.DonationAmount,
    d.DonationYear
FROM Donations d
INNER JOIN Users u ON u.Id = d.UserId
INNER JOIN Celebrations c ON c.Id = d.CelebrationId
WHERE d.DonationYear = @Year
ORDER BY c.CelebrationName

END
