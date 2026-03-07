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

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // GET PLAYER STATS
        [HttpGet("get-player-stats")]
        public async Task<IActionResult> GetPlayerStats(int? userId)
        {
            using var con = GetConnection();

            var stats = await con.QueryAsync(
                "sp_GetPlayerStats",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return Ok(stats);
        }

        // INSERT PLAYER STATS
        [HttpPost("create-player-stats")]
        public async Task<IActionResult> CreatePlayerStats([FromBody] PlayerStat model)
        {
            using var con = GetConnection();

            await con.ExecuteAsync(
                "sp_InsertPlayerStats",
                model,
                commandType: CommandType.StoredProcedure
            );

            return Ok(new { message = "Player stats added successfully" });
        }
    }
}
