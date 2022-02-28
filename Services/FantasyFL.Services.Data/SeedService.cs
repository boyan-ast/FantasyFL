﻿namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Common;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.InputModels.Teams;

    using static FantasyFL.Common.GlobalConstants;

    public class SeedService : ISeedService
    {
        private readonly IFootballDataService footballDataService;
        private readonly IRepository<Gameweek> gameweeksRepository;
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public SeedService(
            IFootballDataService footballDataService,
            IRepository<Gameweek> gameweeksRepository,
            IDeletableEntityRepository<Player> playersRepository,
            IDeletableEntityRepository<Team> teamsRepository,
            IRepository<Stadium> stadiumsRepository)
        {
            this.footballDataService = footballDataService;
            this.gameweeksRepository = gameweeksRepository;
            this.playersRepository = playersRepository;
            this.teamsRepository = teamsRepository;
        }

        public IEnumerable<TeamStadiumDto> TeamsAndStadiumsDto { get; private set; } = new List<TeamStadiumDto>();

        public async Task ImportGameweeks()
        {
            var gameweeks = await this.footballDataService.GetAllRoundsAsync(LeagueExternId, SeasonYear);

            foreach (var gameweek in gameweeks)
            {
                var newGameweek = new Gameweek
                {
                    Name = gameweek,
                    Number = int.Parse(gameweek.Split(" - ")[1]),
                    EndDate = this.ParseGameweekEndDate(CustomData.GameweeksEndDates[gameweek]),
                };

                await this.gameweeksRepository.AddAsync(newGameweek);
            }

            await this.gameweeksRepository.SaveChangesAsync();
        }

        public async Task ImportPlayers()
        {
            var teamIds = this.teamsRepository
                .All()
                .Select(t => new
                {
                    Id = t.Id,
                    ExternId = t.ExternId,
                })
                .ToList();

            foreach (var team in teamIds)
            {
                var squadDto = await this.footballDataService.GetTeamSquadJsonAsync(team.ExternId);

                foreach (var player in squadDto.Players)
                {
                    var newPlayer = new Player
                    {
                        ExternId = player.Id,
                        Name = player.Name,
                        Age = player.Age,
                        Number = player.Number,
                        Position = Enum.Parse<Position>(player.Position, true),
                        TeamId = team.Id,
                    };

                    await this.playersRepository.AddAsync(newPlayer);
                }
            }

            await this.playersRepository.SaveChangesAsync();
        }

        public async Task ImportTeams()
        {
            if (!this.TeamsAndStadiumsDto.Any())
            {
                this.TeamsAndStadiumsDto =
                    (await this.footballDataService.GetTeamsAndStadiumsJsonAsync(LeagueExternId, SeasonYear))
                    .ToList();
            }

            foreach (var teamDto in this.TeamsAndStadiumsDto)
            {
                var team = new Team
                {
                    ExternId = teamDto.Team.Id,
                    Name = teamDto.Team.Name,
                    Logo = teamDto.Team.Logo,
                    Stadium = new Stadium
                    {
                        ExternId = teamDto.Stadium.Id,
                        Name = teamDto.Stadium.Name,
                        City = teamDto.Stadium.City,
                        Capacity = teamDto.Stadium.Capacity,
                        Image = teamDto.Stadium.Image,
                    },
                };

                await this.teamsRepository.AddAsync(team);
            }

            await this.teamsRepository.SaveChangesAsync();
        }

        private DateTime ParseGameweekEndDate(string dateString)
        {
            DateTime.TryParseExact(
                dateString,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date);

            return date;
        }
    }
}
