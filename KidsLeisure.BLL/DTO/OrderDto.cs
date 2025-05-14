namespace KidsLeisure.BLL.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public ProgramTypeDto? ProgramType { get; set; }
        public decimal TotalPrice { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        public List<OrderZoneDto> Zones { get; set; } = new();
        public List<OrderAttractionDto> Attractions { get; set; } = new();
        public List<OrderCharacterDto> Characters { get; set; } = new();
    }
}
