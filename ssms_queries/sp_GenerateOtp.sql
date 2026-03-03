CREATE PROCEDURE sp_GenerateOtp
    @Email NVARCHAR(150),
    @Otp NVARCHAR(10)
AS
BEGIN
    UPDATE Users
    SET Otp = @Otp,
        OtpExpiry = DATEADD(MINUTE, 5, GETDATE())
    WHERE Email = @Email
END
