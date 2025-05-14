namespace KidsLeisure.BLL.DTO
{
    public class OrderAttractionDto
    {
        public int AttractionId { get; set; }
        public string AttractionName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}