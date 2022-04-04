namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IPlayersPointsService
    {
        Task CalculatePoints(int gameweekId);
    }
}
