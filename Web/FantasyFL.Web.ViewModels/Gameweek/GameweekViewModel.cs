namespace FantasyFL.Web.ViewModels.Admin
{
    using System;

    public class GameweekViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public bool IsFinished { get; init; }

        public bool IsImported { get; init; }

        public DateTime? EndDate { get; init; }
    }
}
