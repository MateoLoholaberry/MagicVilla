using AutoMapper;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;

namespace MagicVilla_API
{
    public class MappingConfigProfile : Profile
    {
        public MappingConfigProfile()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}
