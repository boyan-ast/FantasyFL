namespace FantasyFL.Services.Data.InputModels.Lineups
{
    using FantasyFL.Services.Data.InputModels.Fixtures;
    using Newtonsoft.Json;

    public class TeamLineupDto
    {
        public FixtureTeamInfoDto Team { get; init; }

        [JsonProperty("startXI")]
        public LineupPlayerDto[] StartXI { get; init; }

        [JsonProperty("substitutes")]
        public LineupPlayerDto[] Substitutes { get; init; }
    }
}