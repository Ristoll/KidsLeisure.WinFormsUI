namespace KidsLeisure.BLL.DTO
{
    public class OrderZoneDto
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
    }
}