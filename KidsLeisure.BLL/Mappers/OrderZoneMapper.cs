using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class OrderZoneMapper
    {
        public static OrderZoneDto ToDto(DAL.Entities.OrderZoneEntity entity)
        {
            return new OrderZoneDto
            {
                OrderId = entity.OrderId,
                ZoneId = entity.ZoneId
            };
        }

        public static DAL.Entities.OrderZoneEntity ToEntity(OrderZoneDto dto)
        {
            return new DAL.Entities.OrderZoneEntity
            {
                OrderId = dto.OrderId,
                ZoneId = dto.ZoneId
            };
        }
    }
}
