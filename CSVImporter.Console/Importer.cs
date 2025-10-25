using CSVImporter.BLL.Services.Interfaces;
using NLog;

namespace CSVImporter.Console
{
    internal class Importer
    {
        private readonly ITripService _tripService;
        private readonly ILogger _logger;

        public Importer(ITripService tripService, ILogger logger)
        {
            _tripService = tripService;
            _logger = logger;
        }

        public async Task Load(string csvPath)
        {
            try
            {
                _logger.Info("Starting ETL process...");

                _logger.Info("Step 1: Reading CSV file...");
                var trips = await _tripService.ReadCsvAsync(csvPath);
                _logger.Info($"Loaded {trips.Count} rows from CSV");

                _logger.Info("Step 2: Transforming data...");
                var transformedTrips = await _tripService.TransformAsync(trips);
                _logger.Info($"Transformed {transformedTrips.Count} records");

                _logger.Info("Step 3: Inserting into PostgreSQL...");
                await _tripService.InsertToDatabaseAsync(transformedTrips);
                _logger.Info("Data successfully inserted into database!");

                _logger.Info("ETL process completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during ETL process");
            }
        }
    }
}
