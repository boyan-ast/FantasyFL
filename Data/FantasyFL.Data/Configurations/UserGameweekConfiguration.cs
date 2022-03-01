namespace FantasyFL.Data.Configurations
{
    using FantasyFL.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserGameweekConfiguration : IEntityTypeConfiguration<ApplicationUserGameweek>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserGameweek> userGameweek)
        {
            userGameweek
                 .HasKey(x => new { x.UserId, x.GameweekId });
        }
    }
}
