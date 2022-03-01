namespace FantasyFL.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static FantasyFL.Common.GlobalConstants;

    public class ApplicationUserGameweek
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Gameweek))]
        public int GameweekId { get; set; }

        public Gameweek Gameweek { get; set; }

        public int Transfers { get; set; } = 3;

        public int Points { get; set; } = 0;
    }
}
