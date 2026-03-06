namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class Donation
    {
        public int DonationId { get; set; }

        public int UserId { get; set; }

        public int CelebrationId { get; set; }

        public decimal Amount { get; set; }

        public int Year { get; set; }

        public DateTime DonationDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
