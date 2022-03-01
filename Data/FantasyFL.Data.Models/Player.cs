namespace FantasyFL.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FantasyFL.Data.Common.Models;
    using FantasyFL.Data.Models.Enums;

    using static FantasyFL.Common.GlobalConstants;

    public class Player : BaseDeletableModel<int>
    {
        public int ExternId { get; set; }

        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Number { get; set; }

        public Position Position { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public ICollection<PlayerGameweek> PlayerGameweeks { get; set; } = new HashSet<PlayerGameweek>();

        public ICollection<FantasyTeamPlayer> FantasyTeamPlayers { get; set; } = new HashSet<FantasyTeamPlayer>();
    }
}
