namespace FantasyFL.Services.Contracts
{
    using System;

    public interface IParseService
    {
        DateTime ParseDate(string dateString, string format);
    }
}
