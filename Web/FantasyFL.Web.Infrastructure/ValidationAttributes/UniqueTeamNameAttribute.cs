namespace FantasyFL.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using FantasyFL.Services.Data.Contracts;

    public class UniqueTeamNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fantasyTeamsService = (IFantasyTeamsService)validationContext.GetService(typeof(IFantasyTeamsService));
            var nameExists = fantasyTeamsService.FantasyTeamNameExists(value.ToString());

            if (nameExists)
            {
                return new ValidationResult($"Name {value} already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
