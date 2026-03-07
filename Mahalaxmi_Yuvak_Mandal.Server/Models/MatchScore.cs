namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class MatchScore
    {
        public int ScoreId { get; set; }

        public int MatchId { get; set; }

        public string TeamName { get; set; }

        public int Runs { get; set; }

        public int Wickets { get; set; }

        public decimal OversPlayed { get; set; }
    }

}
