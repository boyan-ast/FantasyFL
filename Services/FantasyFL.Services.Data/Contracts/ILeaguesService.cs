namespace FantasyFL.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;

    public interface ILeaguesService
    {
        Task<FantasyLeague> GetLeagueByName(string leagueName);
    }
}
