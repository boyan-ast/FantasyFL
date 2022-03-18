namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Contracts;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Data.Enums;
    using Microsoft.EntityFrameworkCore;

    using static FantasyFL.Common.GlobalConstants;

    public class GameweekImportService : IGameweekImportService
    {
        private readonly IFootballDataService footballDataService;
        private readonly IParseService parseService;
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IRepository<PlayerGameweek> playersGameweeksRepository;
        private readonly IRepository<Gameweek> gameweeksRepository;
        private readonly IRepository<Fixture> fixturesRepository;

        private readonly IDictionary<int, int> playersIds;

        public GameweekImportService(
            IFootballDataService footballDataService,
            IParseService parseService,
            IDeletableEntityRepository<Player> playersRepository,
            IRepository<PlayerGameweek> playersGameweeksRepository,
            IRepository<Gameweek> gameweeksRepository,
            IRepository<Fixture> fixturesRepository)
        {
            this.footballDataService = footballDataService;
            this.parseService = parseService;
            this.playersRepository = playersRepository;
            this.playersGameweeksRepository = playersGameweeksRepository;
            this.gameweeksRepository = gameweeksRepository;
            this.fixturesRepository = fixturesRepository;

            this.playersIds = this.playersRepository
                .All()
                .Select(p => new
                {
                    p.Id,
                    p.ExternId,
                })
                .ToDictionary(p => p.ExternId, p => p.Id);
        }

        public async Task ImportLineups(int gameweekId)
        {
            var fixturesInGameweek = await this.fixturesRepository
                .All()
                .Where(f => f.GameweekId == gameweekId)
                .ToListAsync();

            var allPlayersInGameweekExternIds = new HashSet<int>();

            foreach (var fixture in fixturesInGameweek)
            {
                var lineupsResult = (await this.footballDataService
                    .GetLineupsAsync(fixture.ExternId))
                    .ToList();

                var homeTeamLineupExternIds = lineupsResult[0]
                    .StartXI
                    .Select(p => p.Player.PlayerId);

                var homeTeamSubstitutesExternIds = lineupsResult[0]
                    .Substitutes
                    .Select(p => p.Player.PlayerId);

                var awayTeamLineupExternIds = lineupsResult[1]
                    .StartXI
                    .Select(p => p.Player.PlayerId);

                var awayTeamSubstitutesExternIds = lineupsResult[1]
                    .Substitutes
                    .Select(p => p.Player.PlayerId);

                var startedPlayers = homeTeamLineupExternIds.Union(awayTeamLineupExternIds);
                var reservePlayers = homeTeamSubstitutesExternIds.Union(awayTeamSubstitutesExternIds);
                var allPlayers = startedPlayers.Union(reservePlayers);

                var allPlayersInFixture =
                    this.PlayersInitialAdding(
                        (IEnumerable<int>)allPlayers,
                        (IEnumerable<int>)startedPlayers,
                        (int)gameweekId,
                        allPlayersInGameweekExternIds);

                foreach (var player in allPlayersInFixture)
                {
                    await this.playersGameweeksRepository.AddAsync(player);
                }

                await this.playersGameweeksRepository.SaveChangesAsync();

                allPlayersInGameweekExternIds.UnionWith(allPlayers);
            }
        }

        public async Task ImportEvents(int gameweekId)
        {
            var fixturesInGameweek = await this.fixturesRepository
                .All()
                .Where(f => f.GameweekId == gameweekId)
                .ToListAsync();

            if (fixturesInGameweek.Any(f => f.Status != "FT"))
            {
                await this.UpdateFixturesResults(gameweekId, fixturesInGameweek);
            }

            var playersInGameweek = await this.playersGameweeksRepository
                .All()
                .Include(pg => pg.Player)
                .ThenInclude(p => p.Team)
                .Where(p => p.GameweekId == gameweekId)
                .ToListAsync();

            this.SetIsPlayingForAllPlayersInGameweek(playersInGameweek);

            foreach (var fixture in fixturesInGameweek)
            {
                var fixtureEvents = (await this.footballDataService
                    .GetFixtureEventsAsync(fixture.ExternId))
                    .ToList();

                var homeTeamExternId = fixture.HomeTeam.ExternId;
                var awayTeamExternId = fixture.AwayTeam.ExternId;

                foreach (var matchEvent in fixtureEvents)
                {
                    var eventTime = matchEvent.Time.Elapsed +
                        (int)(matchEvent.Time.Extra != null ? matchEvent.Time.Extra : 0);

                    var isValidEvent = Enum.TryParse(
                        matchEvent.Type,
                        true,
                        out EventType type);

                    if (!isValidEvent)
                    {
                        throw new InvalidOperationException($"Event type '{matchEvent.Type}' is not valid.");
                    }

                    var oponentTeamExternId = matchEvent.Team.Id == homeTeamExternId ?
                        awayTeamExternId : homeTeamExternId;

                    var detail = matchEvent.Detail;

                    var playerExternId = matchEvent.Player.Id;

                    if (!this.playersIds.ContainsKey(playerExternId))
                    {
                        continue;
                    }

                    var player = playersInGameweek
                        .FirstOrDefault(p => p.PlayerId == this.playersIds[playerExternId]);

                    if (player == null)
                    {
                        continue;
                    }

                    if (type == EventType.Goal)
                    {
                        if (detail == "Normal Goal" || detail == "Penalty")
                        {
                            player.Goals += 1;

                            this.UpdateConcededGoalsOfPlayers(oponentTeamExternId, playersInGameweek);
                        }
                        else if (detail == "Own Goal")
                        {
                            player.OwnGoals += 1;

                            this.UpdateConcededGoalsOfPlayers(matchEvent.Team.Id, playersInGameweek);
                        }
                        else if (detail == "Missed Penalty")
                        {
                            player.MissedPenalties += 1;

                            var oponentGoalkeeper = playersInGameweek
                                .Where(p => p.Player.Team.ExternId == oponentTeamExternId)
                                .Where(p => p.Player.Position == Position.Goalkeeper)
                                .Where(p => p.IsPlaying)
                                .First();

                            oponentGoalkeeper.SavedPenalties += 1;
                        }
                        else
                        {
                            throw new InvalidOperationException($"Event detail '{detail}' is not valid.");
                        }
                    }
                    else if (type == EventType.Card)
                    {
                        if (detail == "Yellow Card")
                        {
                            player.YellowCards += 1;
                        }
                        else if (detail == "Second Yellow card" || detail == "Red Card")
                        {
                            player.RedCards += 1;
                            player.IsPlaying = false;

                            var playerStartedMatch = player.InStartingLineup;

                            if (playerStartedMatch)
                            {
                                player.MinutesPlayed = eventTime;
                            }

                            // If the player was a substitute player which was substituted again
                            else
                            {
                                int minutesPlayed = eventTime - (90 - player.MinutesPlayed);
                                player.MinutesPlayed = minutesPlayed > 0 ? minutesPlayed : 0;
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException($"Event detail '{detail}' is not valid.");
                        }
                    }
                    else if (type == EventType.Subst)
                    {
                        var playerStartedMatch = player.InStartingLineup;
                        player.IsPlaying = false;

                        if (playerStartedMatch)
                        {
                            player.MinutesPlayed = eventTime > 90 ? 90 : eventTime;
                        }
                        else
                        {
                            int minutesPlayed = eventTime - (90 - player.MinutesPlayed);
                            player.MinutesPlayed = minutesPlayed > 0 ? minutesPlayed : 0;
                        }

                        var substitutePlayerExternId = matchEvent.Assist.Id ?? 0;

                        if (this.playersIds.ContainsKey(substitutePlayerExternId))
                        {
                            var substitutePlayerGameweek = playersInGameweek
                                .Where(pg => (pg.PlayerId == this.playersIds[substitutePlayerExternId]
                                    && pg.GameweekId == gameweekId))
                                .First();

                            var minutesPlayed = 90 - eventTime;
                            substitutePlayerGameweek.MinutesPlayed = minutesPlayed < 0 ?
                                0 : minutesPlayed;

                            substitutePlayerGameweek.IsPlaying = true;
                        }
                    }
                    else if (type == EventType.Var)
                    {
                        continue;
                    }
                }

                if (fixture.HomeGoals > 0)
                {
                    this.UpdateCleanSheetOfPlayers(awayTeamExternId, playersInGameweek);
                }

                if (fixture.AwayGoals > 0)
                {
                    this.UpdateCleanSheetOfPlayers(homeTeamExternId, playersInGameweek);
                }

                if (fixture.HomeGoals > fixture.AwayGoals)
                {
                    this.UpdatePlayersTeamResult(homeTeamExternId, awayTeamExternId, playersInGameweek);
                }
                else if (fixture.HomeGoals < fixture.AwayGoals)
                {
                    this.UpdatePlayersTeamResult(awayTeamExternId, homeTeamExternId, playersInGameweek);
                }
                else
                {
                    this.UpdatePlayersTeamResult(homeTeamExternId, awayTeamExternId, playersInGameweek, true);
                }

                await this.playersGameweeksRepository.SaveChangesAsync();
            }
        }

        private async Task UpdateFixturesResults(int gameweekId, List<Fixture> fixturesInGameweek)
        {
            var gameweek = this.gameweeksRepository
                .All()
                .FirstOrDefault(gw => gw.Id == gameweekId);

            var fixturesInfo = await this.footballDataService
                .GetAllFixturesByGameweekAsync(gameweek.Name, SeasonYear);

            foreach (var fixtureDto in fixturesInfo)
            {
                var externId = fixtureDto.Fixture.Id;

                var fixture = fixturesInGameweek
                    .FirstOrDefault(f => f.ExternId == externId);

                var status = fixtureDto.Fixture.Status.Status;
                var homeGoals = fixtureDto.Goals.HomeGoals;
                var awayGoals = fixtureDto.Goals.AwayGoals;
                var fixtureDate = this.parseService
                    .ParseDate(fixtureDto.Fixture.Date.Split("T")[0], "yyyy-MM-dd");

                fixture.Status = status;
                fixture.HomeGoals = homeGoals;
                fixture.AwayGoals = awayGoals;
                fixture.Date = fixtureDate;
            }

            await this.fixturesRepository.SaveChangesAsync();
        }

        private void UpdatePlayersTeamResult(
            int winnerExternId,
            int opponentExternId,
            List<PlayerGameweek> playersInGameweek,
            bool isDraw = false)
        {
            if (!isDraw)
            {
                var winnerTeamPlayers = playersInGameweek
                    .Where(pg => pg.Player.Team.ExternId == winnerExternId);

                var opponentTeamPlayers = playersInGameweek
                    .Where(pg => pg.Player.Team.ExternId == opponentExternId);

                foreach (var player in winnerTeamPlayers)
                {
                    player.TeamResult = TeamResult.Won;
                }

                foreach (var player in opponentTeamPlayers)
                {
                    player.TeamResult = TeamResult.Lost;
                }
            }
            else
            {
                var playersInFixture = playersInGameweek
                    .Where(pg => pg.Player.Team.ExternId == winnerExternId
                        || pg.Player.Team.ExternId == opponentExternId);

                foreach (var player in playersInFixture)
                {
                    player.TeamResult = TeamResult.Draw;
                }
            }
        }

        private void UpdateCleanSheetOfPlayers(
            int teamExternId,
            List<PlayerGameweek> playersInGameweek)
        {
            var teamPlayers = playersInGameweek
                .Where(pg => pg.Player.Team.ExternId == teamExternId)
                .Where(p => p.MinutesPlayed > 0);

            foreach (var player in teamPlayers)
            {
                player.CleanSheet = false;
            }
        }

        private void UpdateConcededGoalsOfPlayers(
            int teamExternId,
            List<PlayerGameweek> playersInGameweek)
        {
            var teamPlayers = playersInGameweek
                .Where(pg => pg.Player.Team.ExternId == teamExternId)
                .Where(p => p.IsPlaying);

            foreach (var player in teamPlayers)
            {
                player.ConcededGoals++;
            }
        }

        private void SetIsPlayingForAllPlayersInGameweek(IEnumerable<PlayerGameweek> players)
        {
            foreach (var player in players)
            {
                if (player.InStartingLineup)
                {
                    player.IsPlaying = true;
                }
            }
        }

        private IEnumerable<PlayerGameweek> PlayersInitialAdding(
            IEnumerable<int> playersExternIds,
            IEnumerable<int> startedPlayers,
            int gameweekId,
            HashSet<int> allPlayersInGameweekExternIds)
        {
            var playersGameweek = new List<PlayerGameweek>();

            foreach (var playerExternId in playersExternIds)
            {
                if (!this.playersIds.ContainsKey(playerExternId)
                    || allPlayersInGameweekExternIds.Contains(playerExternId))
                {
                    continue;
                }

                var player = new PlayerGameweek
                {
                    PlayerId = this.playersIds[playerExternId],
                    GameweekId = gameweekId,
                    InStartingLineup = startedPlayers.Contains(playerExternId),
                    IsSubstitute = !startedPlayers.Contains(playerExternId),
                    MinutesPlayed = startedPlayers.Contains(playerExternId) ? 90 : 0,
                    Goals = 0,
                    CleanSheet = true,
                    YellowCards = 0,
                    RedCards = 0,
                    SavedPenalties = 0,
                    ConcededGoals = 0,
                    MissedPenalties = 0,
                    OwnGoals = 0,
                    BonusPoints = 0,
                    TotalPoints = 0,
                };

                playersGameweek.Add(player);
            }

            return playersGameweek;
        }
    }
}
