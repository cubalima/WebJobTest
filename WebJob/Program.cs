using System;
using System.IO;
using Core.Logging;
using Core.Logging.AppInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebJob{
    class Program{
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args){
            Console.WriteLine("Host starting up");
            ReadConfigurationFromSettings();
            PrepareApplicationLogger();
            StartWebJobHost();
        }

        private static void PrepareApplicationLogger(){
            var appInsightsInstrumentationKey = Configuration["AppInsightsKey"];
            var submitter = new AppInsightsEventSubmitter(appInsightsInstrumentationKey);
            Insights.Tracker.DefaultEventBuilder.AddSubmitter(submitter);
            Insights.Tracker.DefaultEventBuilder
                .ToApplication("FleetBack");
        }

        private static void StartWebJobHost(){
            var config = new JobHostConfiguration();
            config.LoggerFactory = new LoggerFactory().AddConsole();
            config.UseTimers();
            var host = new JobHost(config);
            host.RunAndBlock();
        }

        private static void ReadConfigurationFromSettings(){
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }
    }
}
