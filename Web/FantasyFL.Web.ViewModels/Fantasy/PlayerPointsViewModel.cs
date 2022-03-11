namespace FantasyFL.Web.ViewModels.Fantasy
{
    public class PlayerPointsViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int GameweekPoints { get; init; }

        public bool IsPlaying { get; init; }

        public string FantasyTeam { get; init; }
    }
}
