﻿namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Players;
    using Microsoft.EntityFrameworkCore;

    public class PlayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public PlayersService(
            IDeletableEntityRepository<Player> playersRepository,
            IDeletableEntityRepository<Team> teamsRepository)
        {
            this.playersRepository = playersRepository;
            this.teamsRepository = teamsRepository;
        }

        public async Task<List<PlayerListingViewModel>> GetAllByTeam(int id)
        {
            var players = await this.playersRepository
                .All()
                .Where(p => p.TeamId == id)
                .OrderBy(p => p.Position)
                .Select(p => new PlayerListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<List<PlayerListingViewModel>> GetAllPlayers()
        {
            var players = await this.playersRepository
                .All()
                .OrderBy(p => p.TeamId)
                .ThenBy(p => p.Position)
                .Select(p => new PlayerListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<List<PlayerListingViewModel>> GetAllByPosition(Position position)
        {
            var players = await this.playersRepository
                .All()
                .Where(p => p.Position == position)
                .OrderBy(p => p.TeamId)
                .Select(p => new PlayerListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<string> GetPlayerTeamName(int playerId)
        {
            var teamName = await this.playersRepository
                .All()
                .Where(p => p.Id == playerId)
                .Select(p => p.Team.Name)
                .FirstOrDefaultAsync();

            return teamName;
        }
    }
}
