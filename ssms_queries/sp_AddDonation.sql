ALTER PROCEDURE sp_AddDonation
    @UserId INT,
    @CelebrationId INT,
    @Amount DECIMAL(10,2),
    @Year INT
AS
BEGIN
    INSERT INTO Donations
    (
        UserId,
        CelebrationId,
        Amount,
        Year,
        DonationDate,
        CreatedAt
    )
    VALUES
    (
        @UserId,
        @CelebrationId,
        @Amount,
        @Year,
        GETDATE(),
        GETDATE()
    )
END
