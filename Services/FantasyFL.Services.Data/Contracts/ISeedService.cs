﻿namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ISeedService
    {
        public Task ImportTeams();

        public Task ImportPlayers();

        public Task ImportGameweeks();

        public Task ImportFixtures();
    }
}