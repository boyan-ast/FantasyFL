﻿namespace FantasyFL.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.PlayersManagement;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using static FantasyFL.Common.GlobalConstants;

    public class PlayersManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPlayersManagementService playersManagementService;
        private readonly IPlayersService playersService;

        public PlayersManagementController(
            UserManager<ApplicationUser> userManager,
            IPlayersManagementService playersManagementService,
            IPlayersService playersService)
        {
            this.userManager = userManager;
            this.playersManagementService = playersManagementService;
            this.playersService = playersService;
        }

        [Authorize]
        public async Task<IActionResult> PickGoalkeepers()
        {
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickGoalkeepersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
            };

            if (this.TempData.ContainsKey("Players"))
            {
                this.TempData["Players"] = JsonConvert
                    .DeserializeObject<PickPlayersFormModel>(this.TempData["Players"].ToString());
                pickGoalkeepersModel = this.TempData["Players"] as PickPlayersFormModel;
                pickGoalkeepersModel.Players = allPlayers;
            }

            return this.View(pickGoalkeepersModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickDefenders(PickPlayersFormModel model)
        {
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickDefendersFormModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
            };

            return this.View(pickDefendersFormModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickMidfielders(PickPlayersFormModel model)
        {
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickMidfieldersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
                Defenders = model.Defenders,
            };

            return this.View(pickMidfieldersModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickAttackers(PickPlayersFormModel model)
        {
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickAttackersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
                Defenders = model.Defenders,
                Midfielders = model.Midfielders,
            };

            return this.View(pickAttackersModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitTeam(PickPlayersFormModel model)
        {
            var playersTeams = await this.playersService.GetPlayersTeamsCount(model);

            foreach (var (team, playersCount) in playersTeams)
            {
                if (playersCount > MaxCountPlayersFromSameTeam)
                {
                    string errorMessage = $"More than {MaxCountPlayersFromSameTeam} players from {team} selected!";

                    this.ModelState.AddModelError(
                        string.Empty,
                        errorMessage);

                    this.TempData["Alert"] += errorMessage + Environment.NewLine;
                }
            }

            if (!this.ModelState.IsValid)
            {
                this.TempData["Players"] = JsonConvert.SerializeObject(model);

                return this.RedirectToAction("PickGoalkeepers");
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.playersManagementService.AddPlayersToTeam(model, userId);

            return this.Redirect("/Fantasy/PickTeam");
        }
    }
}
