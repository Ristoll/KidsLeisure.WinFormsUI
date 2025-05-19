using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class ZoneMapper
    {
        public static ZoneDto ToDto(DAL.Entities.ZoneEntity entity)
        {
            return new ZoneDto
            {
                Id = entity.ZoneId,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public static DAL.Entities.ZoneEntity ToEntity(ZoneDto dto)
        {
            return new DAL.Entities.ZoneEntity
            {
                ZoneId = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
    }
}
