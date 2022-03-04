namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.Players;

    public interface IPlayersService
    {
        Task<List<PlayerListingViewModel>> GetAllByTeam(int id);
    }
}
