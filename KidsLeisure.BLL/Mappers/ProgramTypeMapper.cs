using KidsLeisure.BLL.DTO;
namespace KidsLeisure.BLL.Mappers
{
    public static class ProgramTypeMapper
    {
        public static ProgramTypeDto ToDto(DAL.Helpers.ProgramType entityType) => entityType switch
        {
            DAL.Helpers.ProgramType.Birthday => ProgramTypeDto.Birthday,
            DAL.Helpers.ProgramType.Custom => ProgramTypeDto.Custom,
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), $"Invalid value: {entityType}")
        };

        public static DAL.Helpers.ProgramType ToEntity(ProgramTypeDto dtoType)
        {
            return dtoType switch
            {
                ProgramTypeDto.Birthday => DAL.Helpers.ProgramType.Birthday,
                ProgramTypeDto.Custom => DAL.Helpers.ProgramType.Custom,
                _ => throw new ArgumentOutOfRangeException(nameof(dtoType), $"Invalid value: {dtoType}")
            };
        }
    }
}