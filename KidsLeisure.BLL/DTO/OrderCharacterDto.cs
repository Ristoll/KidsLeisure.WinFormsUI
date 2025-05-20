namespace KidsLeisure.BLL.DTO
{
    public class OrderCharacterDto : IOrderItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}