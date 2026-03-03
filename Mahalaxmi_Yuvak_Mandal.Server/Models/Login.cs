namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class Login
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }  // Admin / User
    }
}
