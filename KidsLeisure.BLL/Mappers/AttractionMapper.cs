using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class AttractionMapper
    {
        public static AttractionDto ToDto(DAL.Entities.AttractionEntity entity)
        {
            return new AttractionDto
            {
                Id = entity.AttractionId,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public static DAL.Entities.AttractionEntity ToEntity(AttractionDto dto)
        {
            return new DAL.Entities.AttractionEntity
            {
                AttractionId = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
    }
}

