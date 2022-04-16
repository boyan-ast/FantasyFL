namespace FantasyFL.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FantasyFL.Data.Common.Models;

    public class FantasyTeamPlayer : IDeletableEntity
    {
        [Required]
        [ForeignKey(nameof(FantasyTeam))]
        public string FantasyTeamId { get; set; }

        public FantasyTeam FantasyTeam { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public bool IsPlaying { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
