using CSVImporter.DAL.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CSVImporter.DAL
{
    public static class DependencyInjection
    {
        public static void AddDALServices(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton(_ => new ContextDBCSV(connectionString));
        }
    }
}