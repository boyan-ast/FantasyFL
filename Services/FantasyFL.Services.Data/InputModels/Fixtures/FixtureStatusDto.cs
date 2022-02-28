namespace FantasyFL.Services.Data.InputModels.Fixtures
{
    using Newtonsoft.Json;

    public class FixtureStatusDto
    {
        [JsonProperty("short")]
        public string Status { get; init; }

        [JsonProperty("elapsed")]
        public int? MinutesPlayed { get; init; }
    }
}
