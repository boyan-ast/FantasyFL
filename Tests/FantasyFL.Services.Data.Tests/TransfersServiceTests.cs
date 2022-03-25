namespace FantasyFL.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoFixture;
    using AutoFixture.AutoMoq;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class TransfersServiceTests
    {
        [Fact]
        public async Task UserTeamIsEmptyShouldReturnTrueIfTeamExists()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>(),
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            //var mockFantasyTeamPlayersRepo = fixture
            //    .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            //mockFantasyTeamPlayersRepo
            //    .Setup(x => x.SaveChangesAsync())
            //    .Callback(())

            var service = fixture.Create<TransfersService>();

            await service.AddPlayer("user1", 1);

            Assert.Single(userTeam.FantasyTeamPlayers);
            Assert.Equal(1, userTeam.FantasyTeamPlayers.First().PlayerId);
        }
    }
}
