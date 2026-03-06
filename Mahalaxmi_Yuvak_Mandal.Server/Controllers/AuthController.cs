using Mahalaxmi_Yuvak_Mandal.Server.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

    // ================= JWT TOKEN METHOD =================

    private string GenerateJwtToken(int userId, string name, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim("UserId", userId.ToString()),
        new Claim(ClaimTypes.Name, name),
        new Claim(ClaimTypes.Role, role)
    };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ================= LOGIN =================

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
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

        var passwordHash = reader["PasswordHash"]?.ToString();

        if (!BCrypt.Net.BCrypt.Verify(model.Password, passwordHash))
            return BadRequest("Invalid password");

        int userId = Convert.ToInt32(reader["Id"]);
        string name = reader["FullName"]?.ToString() ?? "";
        string role = reader["RoleName"]?.ToString() ?? "";

        var token = GenerateJwtToken(userId, name, role);

        return Ok(new
        {
            Message = "Login successful",
            Token = token,
            UserId = userId,
            Name = name,
            Role = role
        });
    }


}
