namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Administration.Dashboard;

    public interface IGameweekService
    {
        public Task<List<GameweekViewModel>> GetAllAsync();

        public Task GetPlayersData(int gameweekId);

        public Task FinishGameweek(int gameweekId);

        public Gameweek GetCurrent();

        public Gameweek GetNext();
    }
}
