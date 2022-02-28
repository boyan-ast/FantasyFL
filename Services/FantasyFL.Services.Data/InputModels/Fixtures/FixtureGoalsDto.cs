namespace FantasyFL.Services.Data.InputModels.Fixtures
{
    using Newtonsoft.Json;

    public class FixtureGoalsDto
    {
        [JsonProperty("home")]
        public int HomeGoals { get; init; }

        [JsonProperty("away")]
        public int AwayGoals { get; init; }
    }
}
