using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CSVImporter.DAL.Models;

namespace CSVImporter.BLL.Helpers
{
    public class CSVReaderHelper
    {
        public async Task<List<Trip>> ReadTripsFromCsvAsync(string filePath)
        {
            var trips = new List<Trip>();
            var duplicates = new List<Trip>();
            var uniqueKeys = new HashSet<string>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null,
                MissingFieldFound = null,
            };

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                try
                {
                    var pickup = DateTime.Parse(record.tpep_pickup_datetime);
                    var dropoff = DateTime.Parse(record.tpep_dropoff_datetime);
                    var key = $"{pickup:o}-{dropoff:o}-{record.passenger_count}";

                    var trip = new Trip
                    {
                        TpepPickupDatetime = pickup,
                        TpepDropoffDatetime = dropoff,
                        PassengerCount = short.Parse(record.passenger_count),
                        TripDistance = double.Parse(record.trip_distance, CultureInfo.InvariantCulture),
                        StoreAndFwdFlag = NormalizeFlag(record.store_and_fwd_flag),
                        PULocationID = int.Parse(record.PULocationID),
                        DOLocationID = int.Parse(record.DOLocationID),
                        FareAmount = decimal.Parse(record.fare_amount, CultureInfo.InvariantCulture),
                        TipAmount = decimal.Parse(record.tip_amount, CultureInfo.InvariantCulture),
                    };

                    if (!uniqueKeys.Add(key))
                    {
                        duplicates.Add(trip);
                        continue;
                    }

                    trips.Add(trip);
                }
                catch
                { }
            }

            if (duplicates.Any())
                await SaveDuplicatesAsync(duplicates, "duplicates.csv");

            return trips;
        }

        private string NormalizeFlag(string flag)
        {
            return flag?.Trim().ToUpper() switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => flag?.Trim() ?? string.Empty
            };
        }

        private async Task SaveDuplicatesAsync(IEnumerable<Trip> duplicates, string outputPath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            await using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);
            await using var csv = new CsvWriter(writer, config);
            await csv.WriteRecordsAsync(duplicates);
        }
    }
}