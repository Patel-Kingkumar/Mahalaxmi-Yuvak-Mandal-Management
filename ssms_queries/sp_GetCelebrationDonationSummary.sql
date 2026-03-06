-- SP → Celebration Wise Total Donation
CREATE PROCEDURE sp_GetCelebrationDonationSummary
(
    @Year INT
)
AS
BEGIN

SELECT 
    c.CelebrationName,
    SUM(d.DonationAmount) AS TotalDonation
FROM Donations d
INNER JOIN Celebrations c ON c.Id = d.CelebrationId
WHERE d.DonationYear = @Year
GROUP BY c.CelebrationName
ORDER BY c.CelebrationName

END
