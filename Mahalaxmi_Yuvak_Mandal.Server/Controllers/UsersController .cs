using Mahalaxmi_Yuvak_Mandal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient; // FIX: Use Microsoft.Data.SqlClient instead of System.Data.SqlClient
using Dapper;

namespace Mahalaxmi_Yuvak_Mandal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var users = await con.QueryAsync<User>("sp_GetAllUsers", commandType: CommandType.StoredProcedure);

            // Hide sensitive fields before sending to client
            var result = users.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.Role,
                u.IsActive,
                u.CreatedDate
            });

            return Ok(result);
        }
    }
}
