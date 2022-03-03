namespace FantasyFL.Services
{
    using System;
    using System.Globalization;

    using FantasyFL.Services.Contracts;

    public class ParseService : IParseService
    {
        public DateTime ParseDate(string dateString, string format)
        {
            DateTime.TryParseExact(
                dateString,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date);

            return date;
        }
    }
}
