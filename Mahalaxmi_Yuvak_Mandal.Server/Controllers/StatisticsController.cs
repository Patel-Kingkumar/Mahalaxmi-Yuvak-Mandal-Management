using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CricketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StatisticsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // TOTAL MATCHES
        [HttpGet("total-matches")]
        public async Task<IActionResult> GetTotalMatches()
        {
            using var con = GetConnection();

            var result = await con.QueryFirstAsync(
                "sp_TotalMatches",
                commandType: CommandType.StoredProcedure
            );

            return Ok(result);
        }

        // HIGHEST RUN SCORER
        [HttpGet("highest-run-scorer")]
        public async Task<IActionResult> HighestRunScorer()
        {
            using var con = GetConnection();

            var result = await con.QueryFirstAsync(
                "sp_HighestRunScorer",
                commandType: CommandType.StoredProcedure
            );

            return Ok(result);
        }

        // MOST WICKETS
        [HttpGet("most-wickets")]
        public async Task<IActionResult> MostWickets()
        {
            using var con = GetConnection();

            var result = await con.QueryFirstAsync(
                "sp_MostWickets",
                commandType: CommandType.StoredProcedure
            );

            return Ok(result);
        }
    }
}
