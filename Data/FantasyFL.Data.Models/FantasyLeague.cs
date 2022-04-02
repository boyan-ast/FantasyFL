namespace FantasyFL.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FantasyFL.Data.Common.Models;

    using static FantasyFL.Common.GlobalConstants;

    public class FantasyLeague : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();
    }
}
