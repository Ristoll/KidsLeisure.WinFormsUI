using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity> _customerRepository;

        public CustomerEntity? CurrentCustomer { get; set; }

        public CustomerService(IRepository<CustomerEntity> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerEntity> CreateCustomerAsync()
        {
            await _customerRepository.AddAsync(CurrentCustomer!);
            return CurrentCustomer!;
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepository.FindAsync(c => c.CustomerId == customerId);
        }
    }
}
