namespace FantasyFL.Data.Configurations
{
    using FantasyFL.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FantasyTeamPlayerConfiguration : IEntityTypeConfiguration<FantasyTeamPlayer>
    {
        public void Configure(EntityTypeBuilder<FantasyTeamPlayer> fantasyTeamPlayer)
        {
            fantasyTeamPlayer
                .HasKey(x => new { x.FantasyTeamId, x.PlayerId });
        }
    }
}
