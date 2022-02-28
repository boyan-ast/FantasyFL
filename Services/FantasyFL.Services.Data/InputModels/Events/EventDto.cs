namespace FantasyFL.Services.Data.InputModels.Events
{
    using FantasyFL.Services.Data.InputModels.Fixtures;

    public class EventDto
    {
        public TimeInfoDto Time { get; init; }

        public FixtureTeamInfoDto Team { get; init; }

        public PlayerInfoDto Player { get; init; }

        public AssistInfoDto Assist { get; init; }

        public string Type { get; init; }

        public string Detail { get; init; }
    }
}
