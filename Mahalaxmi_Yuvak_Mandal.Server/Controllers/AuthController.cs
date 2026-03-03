using BCrypt.Net;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MimeKit;
using MimeKit;
using System.Data;
using System.Net.Mail;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    }

    // ================= LOGIN =================
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_LoginUser", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Email", model.Email);
        cmd.Parameters.AddWithValue("@RoleName", model.RoleName);

        await con.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.Read())
            return BadRequest("Invalid credentials");

        var passwordHash = reader["PasswordHash"].ToString();

        if (!BCrypt.Net.BCrypt.Verify(model.Password, passwordHash))
            return BadRequest("Invalid password");

        return Ok(new
        {
            Message = "Login successful",
            UserId = reader["Id"],
            Name = reader["FullName"],
            Role = reader["RoleName"]
        });
    }

    // ================= FORGOT PASSWORD =================
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO model)
    {
        var otp = new Random().Next(100000, 999999).ToString();

        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_GenerateOtp", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Email", model.Email);
        cmd.Parameters.AddWithValue("@Otp", otp);

        await con.OpenAsync();
        var rows = await cmd.ExecuteNonQueryAsync();

        if (rows == 0)
            return NotFound("User not found");

        // TODO: Send OTP via email

        return Ok(new { Message = "OTP sent successfully" });
    }

    // ================= VERIFY OTP =================
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest model)
    {
        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_VerifyOtp", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Email", model.Email);
        cmd.Parameters.AddWithValue("@Otp", model.Otp);

        await con.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.Read())
            return BadRequest("Invalid or expired OTP");

        return Ok(new { Message = "OTP verified" });
    }

    // ================= RESET PASSWORD =================
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_ResetPassword", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Email", model.Email);
        cmd.Parameters.AddWithValue("@NewPasswordHash", hashedPassword);

        await con.OpenAsync();
        var rows = await cmd.ExecuteNonQueryAsync();

        if (rows == 0)
            return NotFound("User not found");

        return Ok(new { Message = "Password reset successful" });
    }

    // 16 character app password
    //[HttpPost("send-otp")]
    //public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDTO model)
    //{
    //    // Generate a secure 6-digit OTP
    //    var otp = new Random().Next(100000, 999999).ToString();

    //    using var con = GetConnection();
    //    await con.OpenAsync();

    //    // Step 1: Check if the email exists in Users table
    //    string queryCheckUser = "SELECT Email FROM Users WHERE Email = @Email";
    //    using var checkUserCmd = new SqlCommand(queryCheckUser, con);
    //    checkUserCmd.Parameters.AddWithValue("@Email", model.Email);

    //    var userEmail = await checkUserCmd.ExecuteScalarAsync() as string;

    //    if (string.IsNullOrEmpty(userEmail))
    //    {
    //        return NotFound(new { Message = "User not found" });
    //    }

    //    // Step 2: Save OTP in DB via stored procedure
    //    using var cmd = new SqlCommand("sp_GenerateOtp", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@Email", userEmail);
    //    cmd.Parameters.AddWithValue("@Otp", otp);

    //    var rows = await cmd.ExecuteNonQueryAsync();

    //    if (rows == 0)
    //        return NotFound(new { Message = "Failed to save OTP" });

    //    // Step 3: Send OTP email using the NEW App Password
    //    try
    //    {
    //        var message = new MimeMessage();
    //        message.From.Add(new MailboxAddress("Mahalaxmi Yuvak Mandal", "kingpatel2275479@gmail.com"));
    //        message.To.Add(MailboxAddress.Parse(userEmail));
    //        message.Subject = "Your OTP Code";
    //        message.Body = new TextPart("plain")
    //        {
    //            Text = $"Your OTP code is: {otp}. It is valid for 10 minutes."
    //        };

    //        using var smtp = new MailKit.Net.Smtp.SmtpClient();

    //        // Connect using STARTTLS on port 587
    //        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

    //        // AUTHENTICATION: Use the 16-character key from your screenshot (NO SPACES)
    //        await smtp.AuthenticateAsync("kingpatel2275479@gmail.com", "sqhygvyrhumszrge");

    //        await smtp.SendAsync(message);
    //        await smtp.DisconnectAsync(true);

    //        return Ok(new { Message = "OTP sent successfully" });
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the error if the mail fails but DB was updated
    //        return StatusCode(500, new { Message = "OTP saved but email failed to send.", Error = ex.Message });
    //    }
    //}

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDTO model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
            return BadRequest(new { Message = "Email is required" });

        // 🔐 Secure OTP generation
        var otp = System.Security.Cryptography
                    .RandomNumberGenerator
                    .GetInt32(100000, 999999)
                    .ToString();

        using var con = GetConnection();
        await con.OpenAsync();

        // Step 1: Check if user exists
        string queryCheckUser = "SELECT Email FROM Users WHERE Email = @Email AND IsActive = 1";
        using var checkUserCmd = new SqlCommand(queryCheckUser, con);
        checkUserCmd.Parameters.AddWithValue("@Email", model.Email);

        var userEmail = await checkUserCmd.ExecuteScalarAsync() as string;

        if (string.IsNullOrEmpty(userEmail))
            return NotFound(new { Message = "User not found or inactive" });

        // Step 2: Save OTP in DB
        using var cmd = new SqlCommand("sp_GenerateOtp", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Email", userEmail);
        cmd.Parameters.AddWithValue("@Otp", otp);

        var rows = await cmd.ExecuteNonQueryAsync();

        if (rows == 0)
            return StatusCode(500, new { Message = "Failed to save OTP" });

        // Step 3: Send OTP Email
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                "Mahalaxmi Yuvak Mandal",
                "kingpatel2275479@gmail.com"
            ));

            message.To.Add(MailboxAddress.Parse(userEmail));
            message.Subject = "OTP Verification - Mahalaxmi Yuvak Mandal";

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $@"
        <div style='font-family: Arial, sans-serif; max-width:600px; margin:auto; border:1px solid #eee; padding:20px;'>

            <div style='background: linear-gradient(to right, #f37335, #fdc830); padding:15px; color:white; text-align:center;'>
                <h2 style='margin:0;'>Mahalaxmi Yuvak Mandal</h2>
                <small>Dumas, Surat, Gujarat</small>
            </div>

            <div style='padding:20px; text-align:center;'>
                <h3 style='color:#333;'>OTP Verification</h3>
                <p>Your One Time Password (OTP) is:</p>

                <div style='font-size:30px; font-weight:bold; letter-spacing:5px; 
                            background:#f5f5f5; padding:15px; display:inline-block; 
                            border-radius:5px; color:#333;'>
                    {otp}
                </div>

                <p style='margin-top:20px; color:#555;'>
                    This OTP is valid for <b>10 minutes</b>.
                </p>

                <p style='color:#999; font-size:12px;'>
                    Please do not share this OTP with anyone.
                </p>
            </div>

        </div>";

            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                "kingpatel2275479@gmail.com",
                "sqhygvyrhumszrge" // Gmail App Password
            );

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return Ok(new { Message = "OTP sent successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "OTP saved but email failed to send",
                Error = ex.Message
            });
        }
    }

}
