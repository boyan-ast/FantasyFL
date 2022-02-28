namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.Admin;

    public interface IGameweekService
    {
        public Task<List<GameweekViewModel>> GetAllAsync();
    }
}
