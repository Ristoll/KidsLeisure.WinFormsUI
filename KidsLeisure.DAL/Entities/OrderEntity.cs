using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.DAL.Entities
{
    public class OrderEntity
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public ProgramType ProgramType { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public CustomerEntity? Customer { get; set; }
        public List<OrderZoneEntity> Zones { get; set; }
        public List<OrderAttractionEntity> Attractions { get; set; }
        public List<OrderCharacterEntity> Characters { get; set; }

        public OrderEntity() 
        {
            Zones = new List<OrderZoneEntity>();
            Attractions = new List<OrderAttractionEntity>();
            Characters = new List<OrderCharacterEntity>();
        }
    }
}
