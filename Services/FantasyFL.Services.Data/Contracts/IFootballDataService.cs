namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.InputModels.Events;
    using FantasyFL.Services.Data.InputModels.Fixtures;
    using FantasyFL.Services.Data.InputModels.Lineups;
    using FantasyFL.Services.Data.InputModels.Players;
    using FantasyFL.Services.Data.InputModels.Teams;

    public interface IFootballDataService
    {
        Task<string[]> GetAllRoundsAsync(int league, int season);

        Task<IEnumerable<FixtureInfoDto>> GetAllFixturesByGameweekAsync(string gameweekName, int year);

        Task<IEnumerable<TeamLineupDto>> GetLineupsAsync(int fixtureId);

        Task<IEnumerable<EventDto>> GetFixtureEventsAsync(int fixtureId);

        Task<IEnumerable<TeamStadiumDto>> GetTeamsAndStadiumsJsonAsync(int leagueId, int season);

        Task<TeamPlayersInfoDto> GetTeamSquadJsonAsync(int teamId);
    }
}
