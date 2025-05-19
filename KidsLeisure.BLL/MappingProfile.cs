using AutoMapper;
using KidsLeisure.BLL.DTO;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AttractionEntity, AttractionDto>().ReverseMap();
        CreateMap<CharacterEntity, CharacterDto>().ReverseMap();
        CreateMap<CustomerEntity, CustomerDto>().ReverseMap();
        CreateMap<ZoneEntity, ZoneDto>().ReverseMap();
        CreateMap<OrderAttractionEntity, OrderAttractionDto>().ReverseMap();
        CreateMap<OrderCharacterEntity, OrderCharacterDto>().ReverseMap();
        CreateMap<OrderZoneEntity, OrderZoneDto>().ReverseMap();
        CreateMap<OrderEntity, OrderDto>().ReverseMap();
        CreateMap<ProgramType, ProgramTypeDto>().ReverseMap();
    }
}
