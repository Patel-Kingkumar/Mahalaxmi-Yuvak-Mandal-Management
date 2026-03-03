CREATE PROCEDURE sp_VerifyOtp
    @Email NVARCHAR(150),
    @Otp NVARCHAR(10)
AS
BEGIN
    SELECT Id
    FROM Users
    WHERE Email = @Email
      AND Otp = @Otp
      AND OtpExpiry > GETDATE()
END
