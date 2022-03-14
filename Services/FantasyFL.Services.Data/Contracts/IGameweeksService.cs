namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Administration.Dashboard;

    public interface IGameweeksService
    {
        Task<List<GameweekViewModel>> GetAllAsync();

        Task GetPlayersData(int gameweekId);

        Task FinishGameweek(int gameweekId);

        Task CalculateUserGameweekPoints(string userId, int gameweekId);

        Gameweek GetCurrent();

        Gameweek GetNext();
    }
}
