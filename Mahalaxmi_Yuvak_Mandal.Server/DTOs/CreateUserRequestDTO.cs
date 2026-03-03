namespace Mahalaxmi_Yuvak_Mandal.Server.DTOs
{
    public class CreateUserRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }   // plain password, will be hashed
        public string Role { get; set; }       // Admin, User, etc.
        public bool IsActive { get; set; } = true;  // optional, default true
    }
}
