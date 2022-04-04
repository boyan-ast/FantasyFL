namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ISeedService
    {
        Task ImportTeams();

        Task ImportPlayers();

        Task ImportGameweeks();

        Task ImportFixtures();
    }
}
