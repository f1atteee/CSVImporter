using AutoMapper;
using AutoMapper.Configuration;
using CSVImporter.BLL;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace CSVImporter.Console
{
    internal class Startup
    {
        private const string _connectionName = "DefaultConnection";

        public void ConfigureServices(IServiceCollection services)
        {
            var mapperConfigExpression = new MapperConfigurationExpression();

            var connectionString = GetConnectionStringByName(_connectionName);
            if (connectionString != null)
                services.AddBLLServices(mapperConfigExpression, connectionString);

            services.AddSingleton(new MapperConfiguration(mapperConfigExpression).CreateMapper());
            services.AddSingleton<Importer>();

            ILogger logger = LogManager.GetCurrentClassLogger();
            services.AddSingleton(logger);
        }

        private string GetConnectionStringByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}