using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // FIX: Use Microsoft.Data.SqlClient instead of System.Data.SqlClient
using QuestPDF.Fluent;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.SqlClient;
using Dapper;
using QuestPDF.Helpers; // for Colors
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;   // For predefined colors like Colors.White
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;   // For custom colors

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
            var users = await con.QueryAsync<User>("sp_GetAllUsers", commandType: System.Data.CommandType.StoredProcedure);

            string mandalOrangeHex = "#F37335";

            var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    // Header
                    page.Header()
                        .Background(mandalOrangeHex)
                        .Padding(10)
                        .AlignCenter()
                        .Text("Mahalakshmi Yuvak Mandal")
                        .FontSize(18)
                        .FontColor(Colors.White);

                    // Table with borders
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);   // ID
                            columns.RelativeColumn(3);   // Name
                            columns.RelativeColumn(4);   // Email
                            columns.RelativeColumn(2);   // Role
                            columns.ConstantColumn(50);  // Active
                            columns.RelativeColumn(3);   // Date
                        });

                        // Header row
                        table.Header(header =>
                        {
                            string[] headers = { "ID", "Full Name", "Email", "Role", "Active", "Date" };
                            foreach (var h in headers)
                            {
                                string mandalOrangeHex = "#808080";
                                header.Cell()
                                      .Border(1)
                                      .BorderColor(Colors.Black)
                                      .Padding(5)
                                      //.Background(mandalOrangeHex)   // ✅ Use custom orange
                                      .Text(h)
                                      .FontColor(Colors.Black)    // white text on orange
                                      .SemiBold();
                            }
                        });

                        // Data rows
                        foreach (var user in users)
                        {
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.Id.ToString());
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.FullName);
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.Email);
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.Role ?? "N/A");
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.IsActive ? "Yes" : "No");
                            table.Cell().Border(1).BorderColor(Colors.Black).Padding(5).Text(user.CreatedDate.ToString("dd-MM-yyyy"));
                        }
                    });

                    // Footer
                    page.Footer()
                        .AlignRight()
                        .Text("ESTD. 2005")
                        .FontColor(mandalOrangeHex)
                        .SemiBold();
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", "UsersList.pdf");
        }
    }
}
