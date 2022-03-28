namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Web.ViewModels.Transfers;

    public interface ITransfersService
    {
        Task<TeamTransfersViewModel> GetTransfersList(string userId);

        Task RemovePlayer(string userId, int playerId);

        Task<List<AddPlayerListingViewModel>> GetPlayersToTransfer(string userId, Position removedPlayerPosition);

        Task AddPlayer(string userId, int playerId);
    }
}
