using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Mahalaxmi_Yuvak_Mandal.Server.DTOs;

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
        [HttpGet]
        public async Task<IActionResult> GetDonations()
        {
            using var con = GetConnection();

            var donations = await con.QueryAsync<DonationList>(
                "sp_GetDonations",
                commandType: CommandType.StoredProcedure
            );

            return Ok(donations);
        }

        // ==============================
        // GET DONATION BY ID
        // ==============================
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
    }
}
