namespace FantasyFL.Web
{
    using System;

    using Azure.Extensions.AspNetCore.Configuration.Secrets;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfiguration = config.Build();

                    var keyVaultURL = builtConfiguration["KeyVaultConfig:KVUrl"];
                    var tenantId = builtConfiguration["KeyVaultConfig:TenantId"];
                    var clientId = builtConfiguration["KeyVaultConfig:ClientId"];
                    var clientSecret = builtConfiguration["KeyVaultConfig:ClientSecretId"];

                    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                    var client = new SecretClient(new Uri(keyVaultURL), credential);
                    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
