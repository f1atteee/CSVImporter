using AutoMapper;
using CSVImporter.BLL.Dtos;
using CSVImporter.DAL.Models;

namespace CSVImporter.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TripDto, Trip>().ReverseMap();
        }
    }
}
