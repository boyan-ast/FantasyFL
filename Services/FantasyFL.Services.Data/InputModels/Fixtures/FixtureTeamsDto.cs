namespace FantasyFL.Services.Data.InputModels.Fixtures
{
    using Newtonsoft.Json;

    public class FixtureTeamsDto
    {
        [JsonProperty("home")]
        public FixtureTeamInfoDto HomeTeam { get; init; }

        [JsonProperty("away")]
        public FixtureTeamInfoDto AwayTeam { get; init; }
    }
}
