namespace FantasyFL.Web.ViewModels.Transfers
{
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;

    public class TransferPlayerViewModel : IMapFrom<FantasyTeamPlayer>
    {
        public int PlayerId { get; init; }

        public string PlayerName { get; init; }

        public string PlayerTeamName { get; init; }

        public Position PlayerPosition { get; init; }
    }
}
