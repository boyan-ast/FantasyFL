namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Administration.Dashboard;

    public interface IGameweekService
    {
        Task<List<GameweekViewModel>> GetAllAsync();

        Task GetPlayersData(int gameweekId);

        Task FinishGameweek(int gameweekId);
                
        Gameweek GetCurrent();

        Gameweek GetNext();
    }
}
