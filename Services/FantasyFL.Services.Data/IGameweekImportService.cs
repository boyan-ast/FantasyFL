namespace FantasyFL.Services.Data
{
    using System.Threading.Tasks;

    public interface IGameweekImportService
    {
        public Task ImportFixtures(string gameweekName, int season);

        public Task ImportLineups(int gameweekNumber);

        public Task ImportEvents(int gameweekNumber);
    }
}
