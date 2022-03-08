namespace FantasyFL.Web.ViewModels.FantasyTeam
{
    using System.Collections.Generic;

    using FantasyFL.Web.ViewModels.Players;

    public class PickPlayersFormModel
    {
        public List<PlayerInputModel> Goalkeepers { get; init; }

        public List<PlayerInputModel> Defenders { get; init; }

        public List<PlayerInputModel> Midfielders { get; init; }

        public List<PlayerInputModel> Attackers { get; init; }

        public List<PlayerListingViewModel> Players { get; init; }
    }
}
