namespace FantasyFL.Web.ViewModels.FantasyTeam
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerInputModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }
    }
}
