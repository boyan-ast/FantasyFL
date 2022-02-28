namespace FantasyFL.Services.Data.InputModels.Fixtures
{
    using Newtonsoft.Json;

    public class FixturesResponseDto
    {
        public int Results { get; init; }

        [JsonProperty("response")]
        public FixtureInfoDto[] FixturesInfo { get; init; }
    }
}
