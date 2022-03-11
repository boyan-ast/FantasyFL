namespace FantasyFL.Web.ViewModels.Fantasy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TeamSelectViewModel
    {
        public List<PlayerSelectViewModel> Goalkeepers { get; init; }

        public List<PlayerSelectViewModel> Defenders { get; init; }

        public List<PlayerSelectViewModel> Midfielders { get; init; }

        public List<PlayerSelectViewModel> Attackers { get; init; }

        public List<int> SelectedPlayers { get; init; }
    }
}
