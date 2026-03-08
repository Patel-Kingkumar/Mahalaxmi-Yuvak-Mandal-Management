using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CricketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchScoreController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MatchScoreController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // ADD TEAM SCORE
        [HttpPost("create-score")]
        public async Task<IActionResult> CreateScore([FromBody] MatchScore model)
        {
            using var con = GetConnection();

            await con.ExecuteAsync(
                "sp_InsertMatchScore",
                new
                {
                    model.MatchId,
                    model.TeamName,
                    model.Runs,
                    model.Wickets,
                    model.OversPlayed
                },
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Score added successfully" });
        }


        // GET SCORES BY MATCH
        [HttpGet("get-scores/{matchId}")]
        public async Task<IActionResult> GetScores(int matchId)
        {
            using var con = GetConnection();

            var scores = await con.QueryAsync(
                "sp_GetMatchScores",
                new { MatchId = matchId },
                commandType: CommandType.StoredProcedure
            );

            return Ok(scores);
        }
    }
}
