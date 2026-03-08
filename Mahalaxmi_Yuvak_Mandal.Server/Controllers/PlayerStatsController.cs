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
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PlayerStatsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Insert
        [HttpPost("CreatePlayerStats")]
        public async Task<IActionResult> CreatePlayerStats(PlayerStats stats)
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            // Map Model properties to Stored Procedure parameters
            var param = new
            {
                stats.MatchId,
                stats.PlayerName,
                stats.TeamName,

                Runs = stats.RunsScored,
                BallsFaced = stats.BallsPlayed,
                stats.Fours,
                stats.Sixes,

                OversBowled = stats.OversBowled,
                RunsConceded = stats.RunsGiven,
                Wickets = stats.WicketsTaken
            };

            var result = await con.ExecuteAsync(
                "sp_InsertPlayerStats",
                param,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new
            {
                message = "Player stats inserted successfully",
                rowsAffected = result
            });
        }


        // List
        [HttpGet("GetAllPlayerStats")]
        public async Task<IActionResult> GetAllPlayerStats()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var stats = await con.QueryAsync<PlayerStats>(
                "sp_GetAllPlayerStats",
                commandType: CommandType.StoredProcedure
            );

            return Ok(stats);
        }

        [HttpGet("DownloadPlayerStatsPdf")]
        public async Task<IActionResult> DownloadPlayerStatsPdf()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var con = new SqlConnection(connectionString);

            // GET PLAYER STATS
            var stats = await con.QueryAsync<PlayerStats>(
                "sp_GetAllPlayerStats",
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
                            header.Image(headerImage, ImageScaling.FitWidth);
                    });

                    // TABLE CONTENT
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40); // MatchId
                            columns.RelativeColumn(3);  // Player
                            columns.RelativeColumn(2);  // Team
                            columns.ConstantColumn(40); // Runs
                            columns.ConstantColumn(40); // Balls
                            columns.ConstantColumn(35); // 4s
                            columns.ConstantColumn(35); // 6s
                            columns.ConstantColumn(40); // Wickets
                            columns.ConstantColumn(50); // Overs
                            columns.ConstantColumn(50); // RunsGiven
                            columns.ConstantColumn(40); // Catches
                        });

                        // HEADER
                        table.Header(header =>
                        {
                            string[] headers =
                            {
                        "Match",
                        "Player",
                        "Team",
                        "Runs",
                        "Balls",
                        "4s",
                        "6s",
                        "Wkts",
                        "Overs",
                        "RunsGiven",
                        "Catch"
                    };

                            foreach (var h in headers)
                            {
                                header.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .Text(h)
                                    .SemiBold()
                                    .FontSize(10);
                            }
                        });

                        // DATA
                        foreach (var s in stats)
                        {
                            table.Cell().Border(1).Padding(5).Text(s.MatchId.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.PlayerName);
                            table.Cell().Border(1).Padding(5).Text(s.TeamName);
                            table.Cell().Border(1).Padding(5).Text(s.RunsScored.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.BallsPlayed.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.Fours.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.Sixes.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.WicketsTaken.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.OversBowled.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.RunsGiven.ToString());
                            table.Cell().Border(1).Padding(5).Text(s.Catches.ToString());
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
            var fileName = $"PlayerStats_{now:dd-MMMM-yyyy}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

    }

}
