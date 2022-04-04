namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IGameweekImportService
    {
        Task ImportLineups(int gameweekNumber);

        Task ImportEvents(int gameweekNumber);
    }
}
