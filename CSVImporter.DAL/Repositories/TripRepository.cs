using CSVImporter.DAL.Models;
using CSVImporter.DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CSVImporter.DAL.Repositories;

public class TripRepository : BaseRepository, ITripRepository
{
    private const string DestinationTable = "dbo.Trips";

    public TripRepository(ContextDBCSV context) : base(context)
    { }

    public async Task BulkInsertAsync(IEnumerable<Trip> trips)
    {
        await using var connection = new SqlConnection(_context.GetConnection().ConnectionString);
        await connection.OpenAsync();

        using var bulkCopy = new SqlBulkCopy(connection)
        {
            DestinationTableName = DestinationTable,
            BatchSize = 10000
        };

        var dataTable = CreateDataTable(trips);

        bulkCopy.ColumnMappings.Add(nameof(Trip.TpepPickupDatetime), nameof(Trip.TpepPickupDatetime));
        bulkCopy.ColumnMappings.Add(nameof(Trip.TpepDropoffDatetime), nameof(Trip.TpepDropoffDatetime));
        bulkCopy.ColumnMappings.Add(nameof(Trip.PassengerCount), nameof(Trip.PassengerCount));
        bulkCopy.ColumnMappings.Add(nameof(Trip.TripDistance), nameof(Trip.TripDistance));
        bulkCopy.ColumnMappings.Add(nameof(Trip.StoreAndFwdFlag), nameof(Trip.StoreAndFwdFlag));
        bulkCopy.ColumnMappings.Add(nameof(Trip.PULocationID), nameof(Trip.PULocationID));
        bulkCopy.ColumnMappings.Add(nameof(Trip.DOLocationID), nameof(Trip.DOLocationID));
        bulkCopy.ColumnMappings.Add(nameof(Trip.FareAmount), nameof(Trip.FareAmount));
        bulkCopy.ColumnMappings.Add(nameof(Trip.TipAmount), nameof(Trip.TipAmount));

        await bulkCopy.WriteToServerAsync(dataTable);
    }

    private static DataTable CreateDataTable(IEnumerable<Trip> trips)
    {
        var dataTable = new DataTable();

        dataTable.Columns.Add(nameof(Trip.TpepPickupDatetime), typeof(DateTime));
        dataTable.Columns.Add(nameof(Trip.TpepDropoffDatetime), typeof(DateTime));
        dataTable.Columns.Add(nameof(Trip.PassengerCount), typeof(short));
        dataTable.Columns.Add(nameof(Trip.TripDistance), typeof(double));
        dataTable.Columns.Add(nameof(Trip.StoreAndFwdFlag), typeof(string));
        dataTable.Columns.Add(nameof(Trip.PULocationID), typeof(int));
        dataTable.Columns.Add(nameof(Trip.DOLocationID), typeof(int));
        dataTable.Columns.Add(nameof(Trip.FareAmount), typeof(decimal));
        dataTable.Columns.Add(nameof(Trip.TipAmount), typeof(decimal));

        foreach (var trip in trips)
        {
            dataTable.Rows.Add(
                trip.TpepPickupDatetime,
                trip.TpepDropoffDatetime,
                trip.PassengerCount,
                trip.TripDistance,
                trip.StoreAndFwdFlag,
                trip.PULocationID,
                trip.DOLocationID,
                trip.FareAmount,
                trip.TipAmount
            );
        }
        return dataTable;
    }
}
