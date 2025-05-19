using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class OrderCharacterMapper
    {
        public static OrderCharacterDto ToDto(DAL.Entities.OrderCharacterEntity entity)
        {
            return new OrderCharacterDto
            {
                OrderId = entity.OrderId,
                CharacterId = entity.CharacterId
            };
        }

        public static DAL.Entities.OrderCharacterEntity ToEntity(OrderCharacterDto dto)
        {
            return new DAL.Entities.OrderCharacterEntity
            {
                OrderId = dto.OrderId,
                CharacterId = dto.CharacterId
            };
        }
    }
}
