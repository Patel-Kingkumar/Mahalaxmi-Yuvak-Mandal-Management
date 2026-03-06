IF OBJECT_ID('dbo.sp_UpdateDonation', 'P') IS NOT NULL
DROP PROCEDURE dbo.sp_UpdateDonation
GO

CREATE PROCEDURE dbo.sp_UpdateDonation
    @DonationId INT,
    @UserId INT,
    @CelebrationId INT,
    @Amount DECIMAL(10,2),
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Donations
    SET 
        UserId = @UserId,
        CelebrationId = @CelebrationId,
        Amount = @Amount,
        Year = @Year,
        UpdatedAt = GETDATE()
    WHERE DonationId = @DonationId
END
GO
