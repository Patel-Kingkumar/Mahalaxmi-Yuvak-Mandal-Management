namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{

        public class User
        {
            public int Id { get; set; }

            public string FullName { get; set; }

            public string Email { get; set; }

            public string PasswordHash { get; set; }

            public string Role { get; set; }  // Admin or User

            public string Otp { get; set; }

            public DateTime? OtpExpiry { get; set; }

            public bool IsActive { get; set; } = true;

            public DateTime CreatedDate { get; set; } = DateTime.Now;
        }

}
