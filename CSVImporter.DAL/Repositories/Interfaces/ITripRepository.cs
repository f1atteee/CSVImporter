using CSVImporter.DAL.Models;

namespace CSVImporter.DAL.Repositories.Interfaces
{
    public interface ITripRepository
    {
        Task BulkInsertAsync(IEnumerable<Trip> trips);
    }
}