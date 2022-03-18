using System.ComponentModel.DataAnnotations;

namespace FantasyFL.Web.ViewModels.Fantasy
{
    public class PlayerSelectViewModel
    {
        public int PlayerId { get; init; }

        [Required]
        public string Name { get; init; }

        public bool Selected { get; init; }
    }
}
