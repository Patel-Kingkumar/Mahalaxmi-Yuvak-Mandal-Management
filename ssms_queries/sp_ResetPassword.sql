CREATE PROCEDURE sp_ResetPassword
    @Email NVARCHAR(150),
    @NewPasswordHash NVARCHAR(500)
AS
BEGIN
    UPDATE Users
    SET PasswordHash = @NewPasswordHash,
        Otp = NULL,
        OtpExpiry = NULL
    WHERE Email = @Email
END
