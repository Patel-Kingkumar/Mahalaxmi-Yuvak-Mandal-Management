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
        public async Task<IActionResult> DownloadDonationReport()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var con = new SqlConnection(connectionString);

            // 1️⃣ Get logged-in user's info
            string userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int? userId = !string.IsNullOrEmpty(userIdClaim) ? Convert.ToInt32(userIdClaim) : (int?)null;

            string roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "User";

            // 2️⃣ Call SP: Admin → all records, User → specific user
            var donations = await con.QueryAsync<DonationList>(
                "sp_GetDonationReport",
                new { UserId = roleClaim == "Admin" ? (int?)null : userId },
                commandType: CommandType.StoredProcedure
            );

            // 3️⃣ Load header & stamp images
            var headerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "header.png");
            byte[] headerImage = System.IO.File.Exists(headerPath) ? System.IO.File.ReadAllBytes(headerPath) : null;

            var stampPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "stamp.png");
            byte[] stampImage = System.IO.File.Exists(stampPath) ? System.IO.File.ReadAllBytes(stampPath) : null;

            string mandalOrangeHex = "#F37335";
            string mandalBlueHex = "#1F4E79";

            // 4️⃣ Generate PDF
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
                            header.Image(headerImage, ImageScaling.FitWidth);
                    });

                    // TABLE CONTENT
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        // ⚡ Define 6 columns (must match header/data)
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);   // ID
                            columns.RelativeColumn(2);    // User
                            columns.RelativeColumn(2);    // Celebration
                            columns.ConstantColumn(80);   // Amount
                            columns.ConstantColumn(60);   // Year
                            columns.RelativeColumn(2);    // Date
                        });

                        // TABLE HEADER
                        table.Header(header =>
                        {
                            string[] headers = { "ID", "User", "Celebration", "Amount", "Year", "Date" };
                            foreach (var h in headers)
                                header.Cell().Border(1).Padding(5).Text(h).SemiBold();
                        });

                        // TABLE DATA
                        foreach (var d in donations)
                        {
                            // Explicitly cast dynamic properties
                            int donationId = Convert.ToInt32(d.DonationId);
                            string fullName = Convert.ToString(d.FullName);
                            string celebrationName = Convert.ToString(d.CelebrationName);
                            decimal amount = Convert.ToDecimal(d.Amount);
                            int year = Convert.ToInt32(d.Year);
                            DateTime donationDate = Convert.ToDateTime(d.DonationDate);

                            table.Cell().Border(1).Padding(5).Text(donationId.ToString());
                            table.Cell().Border(1).Padding(5).Text(fullName);
                            table.Cell().Border(1).Padding(5).Text(celebrationName);
                            table.Cell().Border(1).Padding(5).Text($"₹ {amount}");
                            table.Cell().Border(1).Padding(5).Text(year.ToString());
                            table.Cell().Border(1).Padding(5).Text(donationDate.ToString("dd-MMM-yyyy"));
                        }
                    });

                    // FOOTER
                    page.Footer().Height(100).Row(row =>
                    {
                        var now = DateTime.Now;

                        if (stampImage != null)
                            row.ConstantColumn(120).Height(80).AlignMiddle().Image(stampImage, ImageScaling.FitArea);

                        row.RelativeColumn().AlignRight().AlignMiddle().Column(col =>
                        {
                            col.Item().Text("Authorized Signature").Italic().FontSize(12);
                            col.Item().Text("𝓜𝓪𝓱𝓪𝓵𝓪𝔁𝓶𝓲 𝓨𝓾𝓿𝓪𝓴 𝓜𝓪𝓷𝓭𝓪𝓵")
                                .FontSize(10).FontColor(mandalOrangeHex).Italic();
                            col.Item().Text("ESTD 2005").FontSize(10).FontColor(mandalBlueHex);
                            col.Item().PaddingTop(10).Text($"Date: {now:dd-MMMM-yyyy hh:mm tt}").FontSize(10);
                        });
                    });
                });
            }).GeneratePdf();

            var fileName = $"DonationReport_{DateTime.Now:dd-MMMM-yyyy}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}

