namespace Mahalaxmi_Yuvak_Mandal.Server.Models
{
    public class Match
    {
        public int MatchId { get; set; }

        public DateTime MatchDate { get; set; }

        public string GroundName { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }

        public int Overs { get; set; }

        public string MatchType { get; set; }

        public string WinnerTeam { get; set; }
    }

}
