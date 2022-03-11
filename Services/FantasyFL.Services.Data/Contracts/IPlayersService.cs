namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Web.ViewModels.PlayersManagement;
    using FantasyFL.Web.ViewModels.Teams;

    public interface IPlayersService
    {
        Task<List<PlayerListingViewModel>> GetAllByTeam(int id);

        Task<List<PlayerListingViewModel>> GetAllByPosition(Position position);

        Task<List<PlayerListingViewModel>> GetAllPlayers();

        Task<string> GetPlayerTeamName(int playerId);

        // TODO: Find why when remove player first gets id = 0
        Task<int> GetPlayerIdByName(string playerName);

        Task<IDictionary<string, int>> GetPlayersTeamsCount(PickPlayersFormModel model);
    }
}
