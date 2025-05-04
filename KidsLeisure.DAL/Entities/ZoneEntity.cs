using KidsLeisure.DAL.Interfaces;
namespace KidsLeisure.DAL.Entities
{
    public class ZoneEntity : IItemEntity
    {
        public int ZoneId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DisplayName => $"{Name} - {Price} грн";
    }
}