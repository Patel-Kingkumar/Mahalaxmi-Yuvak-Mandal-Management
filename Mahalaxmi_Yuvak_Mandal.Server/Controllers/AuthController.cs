using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
using System.Data;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;

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
    public async Task<IActionResult> Login(LoginRequestDTO model)
    {
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
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
    {
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
}
