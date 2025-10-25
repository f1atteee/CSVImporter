using CSVImporter.BLL.Helpers;
using CSVImporter.BLL.Mappings;
using CSVImporter.BLL.Services.Interfaces;
using CSVImporter.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using CSVImporter.DAL;

namespace CSVImporter.BLL
{
    public static class DependencyInjection
    {
        public static void AddBLLServices(this IServiceCollection services, IMapperConfigurationExpression mapConfigExpression, string connectionString)
        {
            mapConfigExpression.AddProfile<MappingProfile>();

            services.AddScoped<CSVReaderHelper>();
            services.AddScoped<ITripService, TripService>();

            GetDALProject(services, connectionString);
        }

        private static void GetDALProject(IServiceCollection services, string connectionString)
        {
            services.AddDALServices(connectionString);
        }
    }
}
