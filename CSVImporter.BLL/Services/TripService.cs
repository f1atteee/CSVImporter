using AutoMapper;
using CSVImporter.BLL.Dtos;
using CSVImporter.BLL.Helpers;
using CSVImporter.BLL.Services.Interfaces;
using CSVImporter.DAL.Interface;
using CSVImporter.DAL.Models;

namespace CSVImporter.BLL.Services
{
    public class TripService : BaseService, ITripService
    {
        private readonly CSVReaderHelper _csvReader;

        public TripService(IUnitOfWork unitOfWork, IMapper mapper, CSVReaderHelper csvReader) : base(unitOfWork, mapper)
        {
            _csvReader = csvReader;
        }

        public async Task<List<TripDto>> ReadCsvAsync(string filePath)
        {
            var trips = await _csvReader.ReadTripsFromCsvAsync(filePath);
            return _mapper.Map<List<TripDto>>(trips);
        }

        public async Task<List<TripDto>> TransformAsync(List<TripDto> trips)
        {
            foreach (var trip in trips)
            {
                trip.TpepPickupDatetime = TimeZoneInfo.ConvertTimeToUtc(trip.TpepPickupDatetime,
                    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                trip.TpepDropoffDatetime = TimeZoneInfo.ConvertTimeToUtc(trip.TpepDropoffDatetime,
                    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            }

            return await Task.FromResult(trips);
        }

        public async Task InsertToDatabaseAsync(IEnumerable<TripDto> trips)
        {
            var entities = _mapper.Map<List<Trip>>(trips);
            await _unitOfWork.TripRepository.BulkInsertAsync(entities);
        }
    }
}
