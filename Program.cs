using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace mvcapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, configurationBuilder) => {

                                configurationBuilder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);

                                configurationBuilder.AddJsonFile("appsettings.json"
                                                                , optional: false
                                                                , reloadOnChange: true)
                                                    .AddEnvironmentVariables();
                        })
                        .ConfigureLogging((hostingContext, loggingBuilder) => {

                                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                                if (hostingContext.HostingEnvironment.IsDevelopment())
                                {
                                    loggingBuilder.AddConsole();
                                    loggingBuilder.AddDebug();
                                }
                        })
                        .UseStartup<Startup>();
    }
}
