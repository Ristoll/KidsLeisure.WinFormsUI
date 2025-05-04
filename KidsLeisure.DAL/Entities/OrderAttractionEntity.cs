using KidsLeisure.DAL.Interfaces;
namespace KidsLeisure.DAL.Entities
{
    public class OrderAttractionEntity : IOrderItemEntity
    {
        public int OrderId { get; set; }
        public OrderEntity? Order { get; set; }

        public int AttractionId { get; set; }
        public AttractionEntity? Attraction { get; set; }
    }
}