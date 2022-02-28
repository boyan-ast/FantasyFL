namespace FantasyFL.Services.Data.InputModels.Players
{
    public class PlayersResponseDto
    {
        public int Results { get; init; }

        public TeamPlayersInfoDto[] Response { get; init; }
    }
}
