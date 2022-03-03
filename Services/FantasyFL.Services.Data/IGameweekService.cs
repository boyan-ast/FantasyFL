﻿namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Admin;

    public interface IGameweekService
    {
        public Task<List<GameweekViewModel>> GetAllAsync();

        public Task GetPlayersData(int gameweekId);

        public Task FinishGameweek(int gameweekId);

        public Gameweek GetCurrent();

        public Gameweek GetNext();
    }
}
