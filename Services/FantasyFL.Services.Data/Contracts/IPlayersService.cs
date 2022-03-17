namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Web.ViewModels.FirstLeague;
    using FantasyFL.Web.ViewModels.PlayersManagement;

    public interface IPlayersService
    {
        Task<List<PlayerViewModel>> GetAllByTeam(int id);

        Task<List<PlayerViewModel>> GetAllByPosition(Position position);

        Task<List<PlayerViewModel>> GetAllPlayers();

        Task<string> GetPlayerTeamName(int playerId);

        // TODO: Find why when remove player first gets id = 0
        Task<int> GetPlayerIdByName(string playerName);

        Task<IDictionary<string, int>> GetPlayersTeamsCount(PickPlayersFormModel model);
    }
}
