namespace KidsLeisure.BLL.DTO
{
    public class OrderCharacterDto
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}