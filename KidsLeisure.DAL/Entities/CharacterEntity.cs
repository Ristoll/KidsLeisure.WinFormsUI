using KidsLeisure.DAL.Interfaces;

namespace KidsLeisure.DAL.Entities
{
    public class CharacterEntity : IItemEntity
    {
        public int CharacterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DisplayName => $"{Name} - {Price} грн";
    }
}