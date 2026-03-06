ALTER PROCEDURE dbo.sp_DeleteDonation
    @DonationId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Donations WHERE DonationId = @DonationId)
    BEGIN
        DELETE FROM Donations
        WHERE DonationId = @DonationId
    END
END
