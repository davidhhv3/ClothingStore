using AutoMapper;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;

namespace ClothingStore.Infrastructure.Mappings
{
    internal class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }

}
