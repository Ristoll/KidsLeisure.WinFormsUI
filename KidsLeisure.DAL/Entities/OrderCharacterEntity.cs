using KidsLeisure.DAL.Interfaces;
namespace KidsLeisure.DAL.Entities
{
    public class OrderCharacterEntity : IOrderItemEntity
    {
        public int OrderId { get; set; }
        public OrderEntity? Order { get; set; }

        public int CharacterId { get; set; }
        public CharacterEntity? Character { get; set; }
    }
}