namespace FantasyFL.Web.ViewModels.Fantasy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TeamSelectViewModel : IValidatableObject
    {
        public List<PlayerSelectViewModel> Goalkeepers { get; init; }

        public List<PlayerSelectViewModel> Defenders { get; init; }

        public List<PlayerSelectViewModel> Midfielders { get; init; }

        public List<PlayerSelectViewModel> Attackers { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var selectedGoalkeepers = this.Goalkeepers.Count(gk => gk.Selected);
            var selectedDefenders = this.Defenders.Count(gk => gk.Selected);
            var selectedMidfielders = this.Midfielders.Count(gk => gk.Selected);
            var selectedAttackers = this.Attackers.Count(gk => gk.Selected);

            var selectedPlayers = selectedGoalkeepers + selectedDefenders + selectedMidfielders + selectedAttackers;

            if (selectedGoalkeepers != 1)
            {
                yield return new ValidationResult("You must select one goalkeeper.");
            }

            if (selectedPlayers != 11)
            {
                yield return new ValidationResult("You must select exactly 11 players.");
            }
        }
    }
}
