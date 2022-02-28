namespace FantasyFL.Services.Data.InputModels.Teams
{
    using Newtonsoft.Json;

    public class TeamStadiumDto
    {
        public TeamInfoDto Team { get; init; }

        [JsonProperty("venue")]
        public StadiumInfoDto Stadium { get; init; }
    }
}
