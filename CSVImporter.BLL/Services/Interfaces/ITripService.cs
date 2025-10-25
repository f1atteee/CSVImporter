using CSVImporter.BLL.Dtos;

namespace CSVImporter.BLL.Services.Interfaces
{
    public interface ITripService
    {
        Task<List<TripDto>> ReadCsvAsync(string filePath);
        Task<List<TripDto>> TransformAsync(List<TripDto> trips);
        Task InsertToDatabaseAsync(IEnumerable<TripDto> trips);
    }
}