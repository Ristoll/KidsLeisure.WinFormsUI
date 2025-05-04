using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using KidsLeisure.DAL.DBContext;

namespace KidsLeisure.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<CustomerEntity> _customerRepository;
        private readonly LeisureDbContext _context;

        public CustomerEntity? CurrentCustomer { get; set; }

        public CustomerService(IRepository<CustomerEntity> customerRepository, LeisureDbContext context)
        {
            _customerRepository = customerRepository;
            _context = context;
        }

        public async Task<CustomerEntity> CreateCustomerAsync()
        {
            await _customerRepository.AddAsync(CurrentCustomer);
            return CurrentCustomer;
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}
