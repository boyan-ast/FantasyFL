namespace FantasyFL.Services.Data
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
        public Task<string[]> GetAllRoundsAsync(int league, int season);

        public Task<IEnumerable<FixtureInfoDto>> GetAllFixturesByGameweekAsync(int gameweek, int year);

        public Task<IEnumerable<TeamLineupDto>> GetLineupsAsync(int fixtureId);

        public Task<IEnumerable<EventDto>> GetFixtureEventsAsync(int fixtureId);

        public ICollection<int> GetFixturesIds(string round);

        public Task<IEnumerable<TeamStadiumDto>> GetTeamsAndStadiumsJsonAsync(int leagueId, int season);

        public Task<TeamPlayersInfoDto> GetTeamSquadJsonAsync(int teamId);

        public Task SetTeamsTopPlayers();
    }
}
