namespace KidsLeisure.BLL.DTO
{
    public interface IOrderItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}
