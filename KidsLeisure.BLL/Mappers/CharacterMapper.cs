using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class CharacterMapper
    {
        public static CharacterDto ToDto(DAL.Entities.CharacterEntity entity)
        {
            return new CharacterDto
            {
                Id = entity.CharacterId,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public static DAL.Entities.CharacterEntity ToEntity(CharacterDto dto)
        {
            return new DAL.Entities.CharacterEntity
            {
                CharacterId = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
    }
}

