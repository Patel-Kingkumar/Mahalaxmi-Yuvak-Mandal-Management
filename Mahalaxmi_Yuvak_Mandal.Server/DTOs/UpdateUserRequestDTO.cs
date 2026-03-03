namespace Mahalaxmi_Yuvak_Mandal.Server.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }   // Admin, User, etc.
        public bool IsActive { get; set; }
    }
}
