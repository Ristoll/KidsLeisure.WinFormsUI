using KidsLeisure.DAL.Entities;

namespace KidsLeisure.BLL.Interfaces
{
    public interface ICustomerService
    {
        CustomerEntity CurrentCustomer { get; set; }
        Task<CustomerEntity> CreateCustomerAsync();
        Task<List<CustomerEntity>> GetAllCustomersAsync();
        Task<CustomerEntity?> GetCustomerByIdAsync(int customerId);
    }
}
