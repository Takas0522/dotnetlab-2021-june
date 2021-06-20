using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Provider;
using Server.Settings;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Server.Startup))]

namespace Server
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<AzureWebPubSubSetting>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("PubSub").Bind(settings);
               });
            builder.Services.AddSingleton<IAzureWebPubSubServiceProvider, AzureWebPubSubServiceProvider>();
        }
    }
}
