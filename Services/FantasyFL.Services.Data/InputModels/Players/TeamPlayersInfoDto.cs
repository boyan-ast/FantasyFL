namespace FantasyFL.Services.Data.InputModels.Players
{
    using FantasyFL.Services.Data.InputModels.Teams;

    public class TeamPlayersInfoDto
    {
        public TeamInfoDto Team { get; init; }

        public PlayerInfoDto[] Players { get; init; }
    }
}
