namespace FantasyFL.Web.ViewModels.PlayersManagement
{
    using System.Collections.Generic;

    using FantasyFL.Web.ViewModels.FirstLeague;

    public class PickPlayersFormModel
    {
        public List<PlayerInputModel> Goalkeepers { get; set; }

        public List<PlayerInputModel> Defenders { get; set; }

        public List<PlayerInputModel> Midfielders { get; set; }

        public List<PlayerInputModel> Attackers { get; set; }

        public List<PlayerViewModel> Players { get; set; }
    }
}
