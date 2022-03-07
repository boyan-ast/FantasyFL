namespace FantasyFL.Web.ViewModels.FantasyTeam
{
    using FantasyFL.Web.ViewModels.Players;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PickPlayersFormModel
    {
        [Required]
        public string OwnerId { get; init; }

        public List<PlayerInputModel> Goalkeepers { get; init; }

        public List<PlayerInputModel> Defenders { get; init; }

        public List<PlayerInputModel> Midfielders { get; init; }

        public List<PlayerInputModel> Attackers { get; init; }

        public List<PlayerListingViewModel> Players { get; init; }
    }
}
