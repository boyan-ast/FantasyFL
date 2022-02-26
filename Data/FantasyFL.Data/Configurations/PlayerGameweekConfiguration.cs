namespace FantasyFL.Data.Configurations
{
    using FantasyFL.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerGameweekConfiguration : IEntityTypeConfiguration<PlayerGameweek>
    {
        public void Configure(EntityTypeBuilder<PlayerGameweek> playerGameweek)
        {
            playerGameweek
                 .HasKey(x => new { x.PlayerId, x.GameweekId });
        }
    }
}
