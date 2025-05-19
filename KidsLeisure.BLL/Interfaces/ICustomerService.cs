using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Interfaces
{
    public interface ICustomerService
    {
        CustomerDto CurrentCustomer { get; set; }
        Task<CustomerDto> CreateCustomerAsync();
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
    }
}
