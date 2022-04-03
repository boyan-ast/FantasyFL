namespace FantasyFL.Web.ViewModels.PlayersManagement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using FantasyFL.Web.ViewModels.FirstLeague;

    public class PickPlayersFormModel : IValidatableObject
    {
        public List<PlayerInputModel> Goalkeepers { get; set; }

        public List<PlayerInputModel> Defenders { get; set; }

        public List<PlayerInputModel> Midfielders { get; set; }

        public List<PlayerInputModel> Attackers { get; set; }

        public List<PlayerViewModel> Players { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var uniqueGoalkeepers = new HashSet<int>(this.Goalkeepers.Select(gk => gk.Id));

            if (uniqueGoalkeepers.Count < 2)
            {
                yield return new ValidationResult("You should select 2 unique goalkeepers.");
            }

            var uniqueDefenders = new HashSet<int>(this.Defenders.Select(d => d.Id));

            if (uniqueDefenders.Count < 5)
            {
                yield return new ValidationResult("You should select 5 unique defenders.");
            }

            var uniqueMidfielders = new HashSet<int>(this.Midfielders.Select(m => m.Id));

            if (uniqueMidfielders.Count < 5)
            {
                yield return new ValidationResult("You should select 5 unique midfielders.");
            }

            var uniqueAttackers = new HashSet<int>(this.Attackers.Select(a => a.Id));

            if (uniqueAttackers.Count < 3)
            {
                yield return new ValidationResult("You should select 3 unique attackers");
            }
        }
    }
}
