namespace FantasyFL.Data.Configurations
{
    using FantasyFL.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FixtureConfiguration : IEntityTypeConfiguration<Fixture>
    {
        public void Configure(EntityTypeBuilder<Fixture> fixture)
        {
            fixture
                .HasOne(f => f.HomeTeam)
                .WithMany(ht => ht.HomeFixtures)
                .HasForeignKey(f => f.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            fixture
                .HasOne(f => f.AwayTeam)
                .WithMany(ht => ht.AwayFixtures)
                .HasForeignKey(f => f.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
