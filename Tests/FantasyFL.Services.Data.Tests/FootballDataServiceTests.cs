namespace FantasyFL.Services.Data.Tests
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FantasyFL.Services.Contracts;

    using Moq;

    using Xunit;

    public class FootballDataServiceTests
    {
        [Fact]
        public async Task GetTeamsAndStadiumsReturnCorrectResponse()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\teams-172-2021.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetAllTeamsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetTeamsAndStadiumsJsonAsync(172, 2021);

            Assert.Equal("Levski Sofia", result.Skip(2).First().Team.Name);
            Assert.Equal("Vivacom Arena - Georgi Asparuhov", result.Skip(2).First().Stadium.Name);
            Assert.Equal("Ludogorets", result.First().Team.Name);
            Assert.Equal("Razgrad", result.First().Stadium.City);
        }

        [Fact]
        public async Task GetTeamSquadReturnsCorrectResponse()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\players-566.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetSquadAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetTeamSquadJsonAsync(566);

            Assert.Equal("Sergio Padt", result.Players.First().Name);
            Assert.Equal(29, result.Players.Skip(1).First().Age);
            Assert.Equal(566, result.Team.Id);
        }

        [Fact]
        public async Task GetRoundsReturnCorrectResponse()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\gameweeks-2021.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetRoundsJsonAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetAllRoundsAsync(1, 1);

            Assert.Equal("Regular Season - 1", result.First());
            Assert.Equal(26, result.Length);
        }

        [Fact]
        public async Task GetFixturesByGameweekDeserializesCorrectly()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\fixtures-Regular Season - 20-2021.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetFixturesByRoundAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetAllFixturesByGameweekAsync("1", 1);

            Assert.Equal(770996, result.First().Fixture.Id);
            Assert.Equal("Arda Kardzhali", result.First().Teams.HomeTeam.Name);
            Assert.Equal(2, result.First().Goals.AwayGoals);
        }

        [Fact]
        public async Task GetLineupWorksCorrectly()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\lineups-771016.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetLineupsJsonAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetLineupsAsync(771016);

            Assert.Equal("Cherno More Varna", result.First().Team.Name);
            Assert.Equal(11098, result.First().StartXI.First().Player.PlayerId);
        }

        [Fact]
        public async Task GetEventsWorksCorrectly()
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\events-771016.json";

            var json = await File.ReadAllTextAsync(filePath);

            var mockExternalDataService = new Mock<IExternalDataService>();

            mockExternalDataService
                .Setup(x => x.GetFixtureEventsJsonAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(json));

            var service = new FootballDataService(mockExternalDataService.Object);

            var result = await service.GetFixtureEventsAsync(771016);

            Assert.Equal("Cherno More Varna", result.First().Team.Name);
            Assert.Equal(13, result.First().Time.Elapsed);
            Assert.Equal("Yellow Card", result.First().Detail);
        }
    }
}
