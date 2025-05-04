namespace KidsLeisure.DAL.Entities
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }

        public string NickName { get; set; } = "Default";

        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}
