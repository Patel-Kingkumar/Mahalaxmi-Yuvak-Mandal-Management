using Dapper;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    }
}
