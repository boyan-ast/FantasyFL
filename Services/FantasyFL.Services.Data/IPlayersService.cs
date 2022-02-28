namespace FantasyFL.Services.Data
{
    using System.Threading.Tasks;

    public interface IPlayersService
    {
        public Task CalculatePoints(int gameweekId);
    }
}
