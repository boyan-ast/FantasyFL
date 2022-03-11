﻿namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : Controller
    {
        private readonly ITeamsService teamsService;
        private readonly IPlayersService playersService;

        public TeamsController(ITeamsService teamsService, IPlayersService playersService)
        {
            this.teamsService = teamsService;
            this.playersService = playersService;
        }

        public async Task<IActionResult> All()
        {
            var teams = await this.teamsService.GetAll();

            return this.View(teams);
        }

        public async Task<IActionResult> Players(int id)
        {
            var players = await this.playersService.GetAllByTeam(id);

            return this.View(players);
        }
    }
}
