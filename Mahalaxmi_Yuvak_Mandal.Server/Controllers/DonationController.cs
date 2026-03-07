using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;
using System.Security.Claims;

namespace Mahalaxmi_Yuvak_Mandal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DonationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // ==============================
        // GET ALL DONATIONS
        // ==============================
        // ==============================
        // GET ALL DONATIONS OR BY USER
        // ==============================
        [HttpGet("get-donations")]
        public async Task<IActionResult> GetDonations(int? userId)
        {
            using var con = GetConnection();

            var donations = await con.QueryAsync<DonationList>(
                "sp_GetDonations",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return Ok(donations);
        }


        // ==============================
        // GET DONATION BY ID
        // ==============================
        [HttpGet("get-donation/{id}")]
        public async Task<IActionResult> GetDonationById(int id)
        {
            using var con = GetConnection();

            var donation = await con.QueryFirstOrDefaultAsync<Donation>(
                "sp_GetDonationById",
                new { DonationId = id },
                commandType: CommandType.StoredProcedure
            );

            if (donation == null)
                return NotFound("Donation not found");

            return Ok(donation);
        }

        // ==============================
        // CREATE DONATION
        // ==============================
        [HttpPost("create-donation")]
        public async Task<IActionResult> CreateDonation([FromBody] DonationDTO model)
        {
            using var con = GetConnection();

            var parameters = new
            {
                model.UserId,
                model.CelebrationId,
                model.Amount,
                model.Year
            };

            await con.ExecuteAsync(
                "sp_AddDonation",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Donation added successfully" });
        }

        // ==============================
        // UPDATE DONATION
        // ==============================
        [HttpPut("update-donation/{id}")]
        public async Task<IActionResult> UpdateDonation(int id, [FromBody] DonationDTO model)
        {
            using var con = GetConnection();

            var parameters = new
            {
                DonationId = id,
                model.UserId,
                model.CelebrationId,
                model.Amount,
                model.Year
            };

            await con.ExecuteAsync(
                "sp_UpdateDonation",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Donation updated successfully" });
        }

        // ==============================
        // DELETE DONATION
        // ==============================
        [HttpDelete("delete-donation/{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            using var con = GetConnection();

            await con.ExecuteAsync(
                "sp_DeleteDonation",
                new { DonationId = id },
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Donation deleted successfully" });
        }

        [HttpGet("DownloadDonationReport")]
        public async Task<IActionResult> DownloadDonationReport([FromQuery] int userId, [FromQuery] string role)
        {
            // 1. Database Connection
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var con = new SqlConnection(connectionString);

            // If role is Admin, we pass NULL to the SP to get everything
            // If role is User, we pass the actual userId
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            var donations = await con.QueryAsync<DonationList>(
                "sp_GetDonationReport",
                new { UserId = isAdmin ? (int?)null : userId },
                commandType: CommandType.StoredProcedure
            );

            if (donations == null || !donations.Any())
                return NotFound("No donations found for the report.");

            // 3. Fetch Data
            

            // 4. Asset Loading
            var headerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "header.png");
            byte[]? headerImage = System.IO.File.Exists(headerPath) ? System.IO.File.ReadAllBytes(headerPath) : null;

            var stampPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "stamp.png");
            byte[]? stampImage = System.IO.File.Exists(stampPath) ? System.IO.File.ReadAllBytes(stampPath) : null;

            // 5. Generate PDF
            var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    // FIX: Removed .Height(100). Let the image define the height.
                    page.Header().Element(h => {
                        if (headerImage != null)
                            h.Image(headerImage, ImageScaling.FitWidth);
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(60);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            string[] titles = { "ID", "User", "Celebration", "Amount", "Year", "Date" };
                            foreach (var title in titles)
                                header.Cell().Border(1).Padding(5).Text(title).SemiBold();
                        });

                        foreach (var d in donations)
                        {
                            table.Cell().Border(1).Padding(5).Text(d.DonationId.ToString());
                            table.Cell().Border(1).Padding(5).Text(d.FullName ?? "");
                            table.Cell().Border(1).Padding(5).Text(d.CelebrationName ?? "");
                            table.Cell().Border(1).Padding(5).Text($"₹ {d.Amount}");
                            table.Cell().Border(1).Padding(5).Text(d.Year.ToString());
                            table.Cell().Border(1).Padding(5).Text(d.DonationDate.ToString("dd-MMM-yyyy"));
                        }
                    });

                    // FIX: Removed .Height(80) to prevent layout conflicts
                    page.Footer().PaddingTop(10).Row(row =>
                    {
                        if (stampImage != null)
                            row.ConstantColumn(100).Image(stampImage, ImageScaling.FitArea);

                        row.RelativeColumn().AlignRight().Column(col =>
                        {
                            col.Item().Text("Authorized Signature").Italic();
                            col.Item().Text("Mahalaxmi Yuvak Mandal").FontSize(10).SemiBold();
                            col.Item().Text($"Generated: {DateTime.Now:dd-MMM-yyyy hh:mm tt}").FontSize(9);
                        });
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"DonationReport_{DateTime.Now:yyyyMMdd}.pdf");
        }


    }
}

