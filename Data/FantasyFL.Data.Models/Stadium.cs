namespace FantasyFL.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class Stadium : BaseModel<int>
    {
        public int ExternId { get; set; }

        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(DefaultNameMaxLength)]
        public string City { get; set; }

        public int Capacity { get; set; }

        public string Image { get; set; }

        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}
