namespace FantasyFL.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class Team : BaseDeletableModel<int>
    {
        public int ExternId { get; set; }

        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        public string Logo { get; set; }

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();

        [ForeignKey(nameof(Stadium))]
        public int StadiumId { get; set; }

        public Stadium Stadium { get; set; }

        [ForeignKey(nameof(TopPlayer))]
        public int? TopPlayerId { get; set; }

        public Player TopPlayer { get; set; }

        [InverseProperty(nameof(Fixture.HomeTeam))]
        public ICollection<Fixture> HomeFixtures { get; set; } = new HashSet<Fixture>();

        [InverseProperty(nameof(Fixture.AwayTeam))]
        public ICollection<Fixture> AwayFixtures { get; set; } = new HashSet<Fixture>();
    }
}
