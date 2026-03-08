using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Mahalaxmi_Yuvak_Mandal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using var multi = await con.QueryMultipleAsync(
                "sp_GetDashboardStats",
                commandType: CommandType.StoredProcedure
            );

            var totalMatches = await multi.ReadFirstAsync<int>();
            var topRun = await multi.ReadFirstAsync();
            var topWicket = await multi.ReadFirstAsync();

            return Ok(new
            {
                TotalMatches = totalMatches,
                HighestRunScorer = topRun.PlayerName,
                TotalRuns = topRun.TotalRuns,
                MostWicketsPlayer = topWicket.PlayerName,
                TotalWickets = topWicket.TotalWickets
            });
        }

    }
}
