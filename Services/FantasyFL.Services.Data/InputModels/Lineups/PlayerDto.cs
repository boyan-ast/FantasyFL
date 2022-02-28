namespace FantasyFL.Services.Data.InputModels.Lineups
{
    using Newtonsoft.Json;

    public class PlayerDto
    {
        [JsonProperty("id")]
        public int PlayerId { get; init; }

        public string Name { get; init; }

        public int Number { get; init; }

        [JsonProperty("pos")]
        public string Position { get; init; }
    }
}