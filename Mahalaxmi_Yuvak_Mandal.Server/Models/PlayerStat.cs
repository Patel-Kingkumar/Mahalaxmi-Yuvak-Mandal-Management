namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class PlayerStats
    {
        public int PlayerStatId { get; set; }
        public int MatchId { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }

        public int Runs { get; set; }
        public int BallsFaced { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }

        public decimal OversBowled { get; set; }
        public int RunsConceded { get; set; }
        public int Wickets { get; set; }

        public DateTime CreatedDate { get; set; }
    }




}
