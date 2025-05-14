using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(KidsLeisure.DAL.Entities.OrderEntity entity)
        {
            return new OrderDto
            {
                OrderId = entity.OrderId,
                Date = entity.Date,
                ProgramType = entity.ProgramType != null ? ProgramTypeMapper.ToDto(entity.ProgramType.Value) : null,
                TotalPrice = entity.TotalPrice,
                CustomerName = entity.CustomerName,
                CustomerPhone = entity.CustomerPhone,
                CustomerId = entity.CustomerId,
                Zones = entity.Zones.Select(OrderZoneMapper.ToDto).ToList(),
                Attractions = entity.Attractions.Select(OrderAttractionMapper.ToDto).ToList(),
                Characters = entity.Characters.Select(OrderCharacterMapper.ToDto).ToList()
            };
        }

        public static KidsLeisure.DAL.Entities.OrderEntity ToEntity(OrderDto dto)
        {
            return new KidsLeisure.DAL.Entities.OrderEntity
            {
                OrderId = dto.OrderId,
                Date = dto.Date,
                ProgramType = dto.ProgramType != null ? ProgramTypeMapper.ToEntity(dto.ProgramType.Value) : (DAL.Helpers.ProgramType?)null,
                TotalPrice = dto.TotalPrice,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerId = dto.CustomerId,
                Zones = dto.Zones.Select(OrderZoneMapper.ToEntity).ToList(),
                Attractions = dto.Attractions.Select(OrderAttractionMapper.ToEntity).ToList(),
                Characters = dto.Characters.Select(OrderCharacterMapper.ToEntity).ToList()
            };
        }
    }
}
