using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FantasyFL.Web.Areas.Identity.IdentityHostingStartup))]

namespace FantasyFL.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
