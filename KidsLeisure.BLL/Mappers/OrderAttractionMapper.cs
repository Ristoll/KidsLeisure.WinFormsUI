using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Mappers
{
    public static class OrderAttractionMapper
    {
        public static OrderAttractionDto ToDto(KidsLeisure.DAL.Entities.OrderAttractionEntity entity)
        {
            return new OrderAttractionDto
            {
                OrderId = entity.OrderId,
                AttractionId = entity.AttractionId
            };
        }

        public static KidsLeisure.DAL.Entities.OrderAttractionEntity ToEntity(OrderAttractionDto dto)
        {
            return new KidsLeisure.DAL.Entities.OrderAttractionEntity
            {
                OrderId = dto.OrderId,
                AttractionId = dto.AttractionId
            };
        }
    }
}
