namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.Fixtures;

    public interface IFixturesService
    {
        Task<List<FixtureViewModel>> GetAllInCurrentGameweek();

        Task<List<FixtureViewModel>> GetAllInNextGameweek();

        Task<List<FixtureViewModel>> GetFixturesInGameweek(int gameweekId);
    }
}
