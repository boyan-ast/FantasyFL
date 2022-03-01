namespace FantasyFL.Services
{
    using System;

    public interface IParseService
    {
        DateTime ParseDate(string dateString, string format);
    }
}
