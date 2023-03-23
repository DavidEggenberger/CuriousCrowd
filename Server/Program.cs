using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Data;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var allianceLoaderService = host.Services.GetRequiredService<AllianceLoaderService>();
            var dataContextContainer = host.Services.GetRequiredService<DataContextContainer>();

            using (var scope = host.Services.CreateScope())
            {
                //dataContextContainer.Messages.AddRange(await scope.ServiceProvider.GetRequiredService<MessageStreamingService>()
                //    .ReadMessages(10000).ToListAsync());
            }
            dataContextContainer.Alliances = await allianceLoaderService.ReadFirstAlliances();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext) =>
                {
                    hostingContext.AddAzureKeyVault(new Uri("https://curiouscrowdkeyvault.vault.azure.net/"),
                            new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = "476415bc-1e9e-432b-abc9-e11430649068" }));
                            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
