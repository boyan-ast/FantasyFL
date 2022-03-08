namespace FantasyFL.Web.ViewModels.FantasyTeam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FantasyFL.Web.ViewModels.Players;

    public class PickPlayersFormModel : IValidatableObject
    {
        public List<PlayerInputModel> Goalkeepers { get; init; }

        public List<PlayerInputModel> Defenders { get; init; }

        public List<PlayerInputModel> Midfielders { get; init; }

        public List<PlayerInputModel> Attackers { get; init; }

        public List<PlayerListingViewModel> Players { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
        }
    }
}
