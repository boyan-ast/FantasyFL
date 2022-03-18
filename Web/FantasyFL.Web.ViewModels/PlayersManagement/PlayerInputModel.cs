namespace FantasyFL.Web.ViewModels.PlayersManagement
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select a player.")]
        public string Name { get; set; }
    }
}
