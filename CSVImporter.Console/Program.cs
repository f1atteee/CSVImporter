using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace CSVImporter.Console
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var startup = new Startup();
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            LogManager.Setup().LoadConfigurationFromFile("nlog.config");
            var logger = serviceProvider.GetRequiredService<ILogger>();

            try
            {
                logger.Info("Application starting...");
                var mapper = serviceProvider.GetService<IMapper>();

                var importer = serviceProvider.GetRequiredService<Importer>();

                var csvPath = args.Length > 0
                    ? args[0]
                    : "D:\\Data\\sample-cab-data.csv";

                await importer.Load(csvPath);

                logger.Info("Application finished successfully!");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unhandled exception in Main()");
            }
        }
    }
}