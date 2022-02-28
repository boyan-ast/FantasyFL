namespace FantasyFL.Services
{
    using System;
    using System.Globalization;

    public class ParseService : IParseService
    {
        public DateTime ParseDate(string dateString)
        {
            DateTime.TryParseExact(
                dateString,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date);

            return date;
        }
    }
}
