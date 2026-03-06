-- SP → All Users Total Donation
CREATE PROCEDURE sp_GetUsersTotalDonation
(
    @Year INT
)
AS
BEGIN

SELECT 
    u.Id,
    u.FullName,
    SUM(d.DonationAmount) AS TotalDonation
FROM Donations d
INNER JOIN Users u ON u.Id = d.UserId
WHERE d.DonationYear = @Year
GROUP BY u.Id,u.FullName
ORDER BY TotalDonation DESC

END