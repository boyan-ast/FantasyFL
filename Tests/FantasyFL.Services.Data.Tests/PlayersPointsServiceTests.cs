namespace FantasyFL.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;

    using MockQueryable.Moq;

    using Moq;

    using Xunit;

    public class PlayersPointsServiceTests
    {
        [Fact]
        public async Task CalculatePointsShouldWorkAsExpected()
        {
            var playerGameweek = new PlayerGameweek
            {
                PlayerId = 1,
                GameweekId = 1,
                InStartingLineup = true,
                IsSubstitute = false,
                MinutesPlayed = 90,
                Goals = 0,
                CleanSheet = false,
                YellowCards = 0,
                RedCards = 0,
                SavedPenalties = 0,
                ConcededGoals = 2,
                MissedPenalties = 0,
                OwnGoals = 0,
                TeamResult = TeamResult.Won,
                Player = new Player
                {
                    Position = Position.Defender,
                },
            };

            var list = new List<PlayerGameweek>();
            list.Add(playerGameweek);

            var mockRepo = new Mock<IRepository<PlayerGameweek>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new PlayersPointsService(mockRepo.Object);

            await service.CalculatePoints(1);

            Assert.Equal(0, playerGameweek.TotalPoints);
            Assert.Equal(1, playerGameweek.BonusPoints);
        }
    }
}
