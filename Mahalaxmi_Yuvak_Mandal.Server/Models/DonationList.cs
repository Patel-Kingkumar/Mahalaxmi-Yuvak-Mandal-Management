namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class DonationList
    {
        public int DonationId { get; set; }

        public string FullName { get; set; }

        public string CelebrationName { get; set; }

        public decimal Amount { get; set; }

        public int Year { get; set; }

        public DateTime DonationDate { get; set; }
    }
}
