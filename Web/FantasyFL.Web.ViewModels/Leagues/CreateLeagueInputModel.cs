namespace FantasyFL.Web.ViewModels.Leagues
{
    using System.ComponentModel.DataAnnotations;

    using static FantasyFL.Common.GlobalConstants;

    public class CreateLeagueInputModel
    {
        [Required]
        [MinLength(DefaultNameMinLength)]
        [MaxLength(DefaultNameMaxLength)]
        public string Name { get; init; }
    }
}
