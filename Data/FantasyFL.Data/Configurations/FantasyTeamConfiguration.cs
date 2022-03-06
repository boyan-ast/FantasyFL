namespace FantasyFL.Data.Configurations
{
    using FantasyFL.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FantasyTeamConfiguration : IEntityTypeConfiguration<FantasyTeam>
    {
        public void Configure(EntityTypeBuilder<FantasyTeam> fantasyTeam)
        {
            fantasyTeam
                .HasIndex(ft => ft.OwnerId)
                .IsUnique();
        }
    }
}
