namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IGameweekImportService
    {
        public Task ImportLineups(int gameweekNumber);

        public Task ImportEvents(int gameweekNumber);
    }
}
