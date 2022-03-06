namespace FantasyFL.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class FantasyTeam : BaseDeletableModel<string>
    {
        public FantasyTeam()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        public ICollection<FantasyTeamPlayer> FantasyTeamPlayers { get; set; } = new HashSet<FantasyTeamPlayer>();
    }
}
