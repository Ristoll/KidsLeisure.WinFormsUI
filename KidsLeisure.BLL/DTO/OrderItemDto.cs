namespace KidsLeisure.BLL.DTO
{
    public abstract class OrderItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}
