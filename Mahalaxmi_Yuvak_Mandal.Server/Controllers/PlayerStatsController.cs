using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    }

}
