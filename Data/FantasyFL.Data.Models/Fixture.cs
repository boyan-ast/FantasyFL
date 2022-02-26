namespace FantasyFL.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class Fixture : BaseModel<int>
    {
        public int ExternId { get; set; }

        public int GameweekId { get; set; }

        public Gameweek Gameweek { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey(nameof(HomeTeam))]
        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; }

        [ForeignKey(nameof(AwayTeam))]
        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; }

        [Required]
        [MaxLength(FixtureStatusMaxLength)]
        public string Status { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }
    }
}
