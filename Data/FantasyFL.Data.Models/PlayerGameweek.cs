namespace FantasyFL.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using FantasyFL.Data.Common.Models;
    using FantasyFL.Data.Models.Enums;

    public class PlayerGameweek : IDeletableEntity
    {
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        [ForeignKey(nameof(Gameweek))]
        public int GameweekId { get; set; }

        public Gameweek Gameweek { get; set; }

        public bool InStartingLineup { get; set; }

        public bool IsSubstitute { get; set; }

        public int MinutesPlayed { get; set; }

        public int Goals { get; set; }

        public bool CleanSheet { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }

        public int SavedPenalties { get; set; }

        public int ConcededGoals { get; set; }

        public int MissedPenalties { get; set; }

        public int OwnGoals { get; set; }

        public int BonusPoints { get; set; }

        public int TotalPoints { get; set; }

        public TeamResult? TeamResult { get; set; }

        [NotMapped]
        public bool IsPlaying { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
