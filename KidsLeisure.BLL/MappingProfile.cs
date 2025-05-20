using AutoMapper;
using KidsLeisure.BLL.DTO;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AttractionEntity, AttractionDto>().ReverseMap();
            CreateMap<CharacterEntity, CharacterDto>().ReverseMap();
            CreateMap<CustomerEntity, CustomerDto>().ReverseMap();
            CreateMap<ZoneEntity, ZoneDto>().ReverseMap();
            CreateMap<OrderEntity, OrderDto>().ReverseMap();
            CreateMap<ProgramType, ProgramTypeDto>().ReverseMap();
            CreateMap<CharacterEntity, OrderCharacterDto>().ReverseMap();
            CreateMap<AttractionEntity, OrderAttractionDto>().ReverseMap();
            CreateMap<ZoneEntity, OrderZoneDto>().ReverseMap();

            CreateMap<OrderEntity, OrderDto>()
                .ForMember(dest => dest.Attractions, opt => opt.MapFrom(src => src.Attractions))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters))
                .ForMember(dest => dest.ProgramType, opt => opt.MapFrom(src => (ProgramTypeDto)src.ProgramType));

            CreateMap<OrderDto, OrderEntity>()
                .ForMember(dest => dest.Attractions, opt => opt.MapFrom(src => src.Attractions))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => src.Characters))
                .ForMember(dest => dest.ProgramType, opt => opt.MapFrom(src => (ProgramType)src.ProgramType));

            CreateMap<OrderAttractionEntity, OrderAttractionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AttractionId))
                .ReverseMap()
                .ForMember(dest => dest.AttractionId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrderCharacterEntity, OrderCharacterDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CharacterId))
                .ReverseMap()
                .ForMember(dest => dest.CharacterId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrderZoneEntity, OrderZoneDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ZoneId))
                .ReverseMap()
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.Id));
        }
    }
}