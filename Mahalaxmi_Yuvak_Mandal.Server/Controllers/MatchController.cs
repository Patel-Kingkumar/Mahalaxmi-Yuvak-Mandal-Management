using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;


namespace CricketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // GET ALL MATCHES
        [HttpGet("get-matches")]
        public async Task<IActionResult> GetMatches()
        {
            using var con = GetConnection();

            var matches = await con.QueryAsync(
                "sp_GetMatches",
                commandType: CommandType.StoredProcedure
            );

            return Ok(matches);
        }

        // GET MATCH BY ID
        [HttpGet("get-match/{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            using var con = GetConnection();

            var match = await con.QueryFirstOrDefaultAsync(
                "sp_GetMatchById",
                new { MatchId = id },
                commandType: CommandType.StoredProcedure
            );

            return Ok(match);
        }

        // CREATE MATCH
        [HttpPost("create-match")]
        public async Task<IActionResult> CreateMatch([FromBody] Match model)
        {
            try
            {
                using var con = GetConnection();

                var param = new
                {
                    model.MatchDate,
                    model.GroundName,
                    model.TeamA,
                    model.TeamB,
                    model.Overs,
                    model.MatchType,
                    model.WinnerTeam
                };

                await con.ExecuteAsync(
                    "sp_InsertMatch",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                return Ok(new { message = "Match created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // UPDATE MATCH
        [HttpPut("update-match/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] Match model)
        {
            using var con = GetConnection();

            var parameters = new
            {
                MatchId = id,
                model.MatchDate,
                model.GroundName,
                model.TeamA,
                model.TeamB,
                model.Overs,
                model.MatchType,
                model.WinnerTeam
            };

            await con.ExecuteAsync(
                "sp_UpdateMatch",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Match updated successfully" });
        }

        // DELETE MATCH
        [HttpDelete("delete-match/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            using var con = GetConnection();

            await con.ExecuteAsync(
                "sp_DeleteMatch",
                new { MatchId = id },
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Match deleted successfully" });
        }

        [HttpGet("DownloadMatchesPdf")]
        public async Task<IActionResult> DownloadMatchesPdf()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var con = new SqlConnection(connectionString);

            // GET ALL MATCHES
            var matches = await con.QueryAsync<Match>(
                "sp_GetMatches",
                commandType: System.Data.CommandType.StoredProcedure
            );

            // HEADER IMAGE
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

            // STAMP IMAGE
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

                    // HEADER
                    page.Header().Height(150).Element(header =>
                    {
                        if (headerImage != null)
                        {
                            header.Image(headerImage, ImageScaling.FitWidth);
                        }
                    });

                    // CONTENT
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                        });

                        // TABLE HEADER
                        table.Header(header =>
                        {
                            string[] headers = {
                        "ID",
                        "Match Date",
                        "Team A",
                        "Team B",
                        "Overs",
                        "Type",
                        "Winner"
                    };

                            foreach (var h in headers)
                            {
                                header.Cell()
                                      .Border(1)
                                      .BorderColor(Colors.Black)
                                      .Padding(5)
                                      .Text(h)
                                      .SemiBold();
                            }
                        });

                        // TABLE DATA
                        foreach (var match in matches)
                        {
                            table.Cell().Border(1).Padding(5).Text(match.MatchId.ToString());
                            table.Cell().Border(1).Padding(5).Text(match.MatchDate.ToString("dd-MM-yyyy"));
                            table.Cell().Border(1).Padding(5).Text(match.TeamA);
                            table.Cell().Border(1).Padding(5).Text(match.TeamB);
                            table.Cell().Border(1).Padding(5).Text(match.Overs.ToString());
                            table.Cell().Border(1).Padding(5).Text(match.MatchType ?? "N/A");
                            table.Cell().Border(1).Padding(5).Text(match.WinnerTeam ?? "TBD");
                        }
                    });

                    // FOOTER
                    page.Footer().Height(100).Row(row =>
                    {
                        var now = DateTime.Now;

                        // STAMP
                        if (stampImage != null)
                        {
                            row.ConstantColumn(120)
                                .Height(80)
                                .AlignMiddle()
                                .Image(stampImage, ImageScaling.FitArea);
                        }

                        // SIGNATURE
                        row.RelativeColumn()
                            .AlignRight()
                            .AlignMiddle()
                            .Column(col =>
                            {
                                col.Item().Text("Authorized Signature").Italic().FontSize(12);

                                col.Item().Text("𝓜𝓪𝓱𝓪𝓵𝓪𝔁𝓶𝓲 𝓨𝓾𝓿𝓪𝓴 𝓜𝓪𝓷𝓭𝓪𝓵")
                                    .FontSize(10)
                                    .FontColor(mandalOrangeHex)
                                    .Italic();

                                col.Item().Text("ESTD 2005")
                                    .FontSize(10)
                                    .FontColor(mandalBlueHex);

                                col.Item().PaddingTop(10)
                                    .Text($"Date: {now:dd-MMMM-yyyy hh:mm tt}")
                                    .FontSize(10);
                            });
                    });
                });
            }).GeneratePdf();

            var now = DateTime.Now;
            var fileName = $"MatchList_{now:dd-MMMM-yyyy}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}
