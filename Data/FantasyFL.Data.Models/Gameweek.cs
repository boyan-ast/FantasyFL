namespace FantasyFL.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class Gameweek : BaseModel<int>
    {
        [Required]
        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        public int Number { get; set; }

        public int Season { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsFinished { get; set; }

        public bool IsImported { get; set; }

        public ICollection<Fixture> Fixtures { get; set; } = new HashSet<Fixture>();

        public ICollection<PlayerGameweek> PlayerGameweeks { get; set; } = new HashSet<PlayerGameweek>();

        public ICollection<ApplicationUserGameweek> ApplicationUsersGameweeks { get; set; } = new HashSet<ApplicationUserGameweek>();
    }
}
