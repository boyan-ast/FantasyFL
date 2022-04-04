namespace FantasyFL.Web.ViewModels.Fantasy
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerSelectViewModel
    {
        public int PlayerId { get; init; }

        [Required]
        public string Name { get; init; }

        public bool Selected { get; init; }
    }
}
