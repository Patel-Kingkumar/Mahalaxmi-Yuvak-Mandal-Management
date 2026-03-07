using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // FIX: Use Microsoft.Data.SqlClient instead of System.Data.SqlClient
using QuestPDF.Fluent;
using QuestPDF.Helpers; // for Colors
using QuestPDF.Infrastructure;
using System.Data;

namespace Mahalaxmi_Yuvak_Mandal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var users = await con.QueryAsync<User>("sp_GetAllUsers", commandType: CommandType.StoredProcedure);

            // Hide sensitive fields before sending to client
            var result = users.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.Role,
                u.IsActive,
                u.CreatedDate
            });

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO model)
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            // Check if email already exists
            var existingUser = await con.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email",
                new { Email = model.Email }
            );
            if (existingUser != null)
                return BadRequest("Email already exists");

            // Get RoleId from Roles table
            var roleId = await con.QueryFirstOrDefaultAsync<int?>(
                "SELECT Id FROM Roles WHERE RoleName = @Role",
                new { Role = model.Role }
            );

            if (roleId == null)
                return BadRequest("Invalid role");

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Call SP to insert
            await con.ExecuteAsync(
                "sp_CreateUser",
                new
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    RoleId = roleId.Value,
                    IsActive = model.IsActive
                },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return Ok(new { Message = "User created successfully" });
        }


        // GET by Id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
            public async Task<IActionResult> GetUserById(int id)
            {
                using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var user = await con.QueryFirstOrDefaultAsync<User>(
                    "sp_GetUserById",
                    new { Id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (user == null) return NotFound("User not found");

                return Ok(user);
            }

        // PUT /api/Users/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDTO model)
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            // Get RoleId from Roles table
            var roleId = await con.QueryFirstOrDefaultAsync<int?>(
                "SELECT Id FROM Roles WHERE RoleName = @Role",
                new { Role = model.Role }
            );

            if (roleId == null)
                return BadRequest("Invalid role");

            var rows = await con.ExecuteAsync(
                "sp_UpdateUser",
                new
                {
                    Id = id,
                    FullName = model.FullName,
                    Email = model.Email,
                    RoleId = roleId.Value,
                    IsActive = model.IsActive
                },
                commandType: System.Data.CommandType.StoredProcedure
            );

            if (rows == 0)
                return NotFound("User not found");

            return Ok(new { Message = "User updated successfully" });
        }

        // DELETE /api/Users/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var rows = await con.ExecuteAsync(
                    "sp_DeleteUser",
                    new { Id = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (rows == 0) return NotFound("User not found");

                return Ok(new { Message = "User deleted successfully" });
            }

        [HttpGet("DownloadUsersPdf")]
        public async Task<IActionResult> DownloadUsersPdf()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var con = new SqlConnection(connectionString);

            // Get ALL users
            var users = await con.QueryAsync<User>(
                "sp_GetAllUsers",
                commandType: System.Data.CommandType.StoredProcedure
            );

            // HEADER IMAGE PATH
            var headerPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "header.png"
            );

            byte[] headerImage = null;

            if (System.IO.File.Exists(headerPath))
            {
                headerImage = System.IO.File.ReadAllBytes(headerPath);
            }

            // STAMP IMAGE PATH
            var stampPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "stamp.png"
            );

            byte[] stampImage = null;

            if (System.IO.File.Exists(stampPath))
            {
                stampImage = System.IO.File.ReadAllBytes(stampPath);
            }

            string mandalOrangeHex = "#F37335";
            string mandalBlueHex = "#1F4E79";

            var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    // HEADER IMAGE
                    page.Header().Height(150).Element(header =>
                    {
                        if (headerImage != null)
                        {
                            header.Image(headerImage, ImageScaling.FitWidth);
                        }
                    });

                    // CONTENT TABLE
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(50);
                            columns.RelativeColumn(3);
                        });

                        // TABLE HEADER
                        table.Header(header =>
                        {
                            string[] headers = { "ID", "Full Name", "Email", "Role", "Active", "Date" };

                            foreach (var h in headers)
                            {
                                header.Cell()
                                      .Border(1)
                                      .BorderColor(Colors.Black)
                                      .Padding(5)
                                      .Text(h)
                                      .FontColor(Colors.Black)
                                      .SemiBold();
                            }
                        });

                        // TABLE DATA
                        foreach (var user in users)
                        {
                            table.Cell().Border(1).Padding(5).Text(user.Id.ToString());
                            table.Cell().Border(1).Padding(5).Text(user.FullName);
                            table.Cell().Border(1).Padding(5).Text(user.Email);
                            table.Cell().Border(1).Padding(5).Text(user.Role ?? "N/A");
                            table.Cell().Border(1).Padding(5).Text(user.IsActive ? "Yes" : "No");
                            table.Cell().Border(1).Padding(5).Text(user.CreatedDate.ToString("dd-MM-yyyy"));
                        }
                    });

                    // FOOTER
                    page.Footer().Height(100).Row(row =>
                    {
                        var now = DateTime.Now;

                        // LEFT → Stamp
                        if (stampImage != null)
                        {
                            row.ConstantColumn(120)
                                .Height(80)
                                .AlignMiddle()
                                .Image(stampImage, ImageScaling.FitArea);
                        }

                        // RIGHT → Signature
                        row.RelativeColumn()
                            .AlignRight()
                            .AlignMiddle()
                            .Column(col =>
                            {
                                col.Item().Text("Authorized Signature").Italic().FontSize(12);

                                col.Item().Text("𝓜𝓪𝓱𝓪𝓵𝓪𝔁𝓶𝓲 𝓨𝓾𝓿𝓪𝓵 𝓜𝓪𝓷𝓭𝓪𝓵")
                                    .FontSize(10)
                                    .FontColor(mandalOrangeHex)
                                    .Italic();
                                col.Item().Text("ESTD 2005")
                                    .FontSize(10)
                                    .FontColor(mandalBlueHex);

                                col.Item().PaddingTop(10).Text($"Date: {now:dd-MMMM-yyyy hh:mm tt}")
                                    .FontSize(10);

                                
                            });
                    });

                });
            }).GeneratePdf();

            var now = DateTime.Now;
            var fileName = $"UsersList_{now:dd-MMMM-yyyy}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }


    }
}

