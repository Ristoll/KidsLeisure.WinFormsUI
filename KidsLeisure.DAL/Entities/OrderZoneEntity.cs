using KidsLeisure.DAL.Interfaces;

namespace KidsLeisure.DAL.Entities
{
    public class OrderZoneEntity : IOrderItemEntity
    {
        public int OrderId { get; set; }
        public OrderEntity? Order { get; set; }

        public int ZoneId { get; set; }
        public ZoneEntity? Zone { get; set; }
    }
}