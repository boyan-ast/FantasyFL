namespace FantasyFL.Services.Data.InputModels.Gameweeks
{
    using Newtonsoft.Json;

    public class GameweeksResponseDto
    {
        public int Results { get; init; }

        [JsonProperty("response")]
        public string[] Rounds { get; init; }
    }
}
