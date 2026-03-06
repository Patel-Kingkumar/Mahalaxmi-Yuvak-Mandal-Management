using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
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

        //    [HttpGet("DownloadUsersPdf")]
        //    public async Task<IActionResult> DownloadUsersPdf()
        //    {
        //        using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //        var users = await con.QueryAsync<User>("sp_GetAllUsers", commandType: CommandType.StoredProcedure);

        //        using var ms = new MemoryStream();
        //        var writer = new PdfWriter(ms);
        //        var pdf = new PdfDocument(writer);
        //        var document = new Document(pdf);

        //        // -- Header section like your receipt --

        //        // Add the top orange banner with text
        //        var orangeColor = new DeviceRgb(243, 115, 53); // #f37335 approx

        //        // Background rectangle for header
        //        var headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1 })).UseAllAvailableWidth();
        //        headerTable.SetBackgroundColor(orangeColor);
        //        headerTable.SetPadding(10);
        //        headerTable.AddCell(new Cell()
        //            .Add(new Paragraph("Mahalakshmi Yuvak Mandal")
        //                .SetFontSize(18)
        //                .SetFontColor(ColorConstants.White)
        //                .SetBold())
        //            .SetBorder(Border.NO_BORDER)
        //            .SetTextAlignment(TextAlignment.CENTER));
        //        headerTable.AddCell(new Cell()
        //            .Add(new Paragraph("Dumas, Surat, Gujarat")
        //                .SetFontSize(12)
        //                .SetFontColor(ColorConstants.White))
        //            .SetBorder(Border.NO_BORDER)
        //            .SetTextAlignment(TextAlignment.CENTER));
        //        document.Add(headerTable);

        //        document.Add(new Paragraph("\n"));

        //        // Title section
        //        document.Add(new Paragraph("Users List").SetFontSize(16).SetBold().SetTextAlignment(TextAlignment.CENTER));
        //        document.Add(new Paragraph($"Generated On: {DateTime.Now:dd-MM-yyyy HH:mm}").SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
        //        document.Add(new Paragraph("\n"));

        //        // -- User Table --

        //        var table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 5, 6, 3, 2, 4 })).UseAllAvailableWidth();

        //        // Table Header
        //        var headers = new[] { "ID", "Full Name", "Email", "Role", "Active", "Created Date" };
        //        foreach (var h in headers)
        //        {
        //            table.AddHeaderCell(new Cell().Add(new Paragraph(h).SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
        //        }

        //        // Table Rows
        //        foreach (var user in users)
        //        {
        //            table.AddCell(user.Id.ToString());
        //            table.AddCell(user.FullName);
        //            table.AddCell(user.Email);
        //            table.AddCell(user.Role ?? "N/A");
        //            table.AddCell(user.IsActive ? "Yes" : "No");
        //            table.AddCell(user.CreatedDate.ToString("dd-MM-yyyy"));
        //        }

        //        document.Add(table);

        //        document.Add(new Paragraph("\n\n"));

        //        // Footer - Signature and ESTD Stamp style

        //        // Add signature placeholder
        //        var signature = new Paragraph("Authorized Signature")
        //            .SetFontSize(12)
        //            .SetTextAlignment(TextAlignment.RIGHT);

        //        var signatureLine = new Paragraph("______________________")
        //            .SetFontSize(12)
        //            .SetTextAlignment(TextAlignment.RIGHT);

        //        document.Add(signature);
        //        document.Add(signatureLine);

        //        document.Add(new Paragraph("\n"));

        //        // Add circular ESTD 2005 stamp on bottom right
        //        // For a perfect circular stamp with text, you can create a custom Canvas or use an image.

        //        // For simplicity, let's just add a styled text block mimicking a stamp:

        //        var estdParagraph = new Paragraph("MAHALAKSHMI YUVAK MANDAL\nREGD.\nESTD. 2005")
        //            .SetFontSize(10)
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFontColor(orangeColor)
        //            .SetBold();

        //        var estdTable = new Table(1).SetFixedPosition(450, 50, 100).UseAllAvailableWidth();
        //        estdTable.AddCell(new Cell().Add(estdParagraph).SetBorder(new SolidBorder(orangeColor, 2)).SetTextAlignment(TextAlignment.CENTER));
        //        document.Add(estdTable);

        //        // Close document
        //        document.Close();

        //        var bytes = ms.ToArray();

        //        return File(bytes, "application/pdf", "UsersList.pdf");
        //    }
        //}

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

                                col.Item().Text("MYM")
                                    .FontSize(20)
                                    .FontColor(mandalOrangeHex)
                                    .Italic();

                                col.Item().Text($"Date: {now:dd-MMMM-yyyy}")
                                    .FontSize(10);

                                col.Item().Text("ESTD 2005")
                                    .FontSize(10)
                                    .FontColor(mandalBlueHex);
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

