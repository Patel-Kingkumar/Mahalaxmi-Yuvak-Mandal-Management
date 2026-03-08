namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class PlayerStats
    {
        public int MatchId { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }

        public int RunsScored { get; set; }
        public int BallsPlayed { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }

        public int WicketsTaken { get; set; }
        public decimal OversBowled { get; set; }
        public int RunsGiven { get; set; }
        public int Catches { get; set; }
    }





}
