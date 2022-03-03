namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IPlayersService
    {
        public Task CalculatePoints(int gameweekId);
    }
}
