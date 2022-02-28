namespace FantasyFL.Services.Data
{
    using System.Threading.Tasks;

    public interface ISeedService
    {
        public Task ImportTeams();

        public Task ImportStadiums();

        public Task ImportPlayers();

        public Task ImportGameweeks();
    }
}
