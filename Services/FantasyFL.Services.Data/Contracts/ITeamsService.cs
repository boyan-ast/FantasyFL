namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.Teams;

    public interface ITeamsService
    {
        Task<List<TeamListingViewModel>> GetAll();
    }
}
