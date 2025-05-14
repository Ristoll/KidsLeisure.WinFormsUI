using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerEntity? CurrentCustomer { get; set; }

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerEntity> CreateCustomerAsync()
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();

            await repository.AddAsync(CurrentCustomer!);
            await _unitOfWork.SaveChangesAsync();

            return CurrentCustomer!;
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();
            return await repository.GetAllAsync();
        }

        public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();
            return await repository.FindAsync(c => c.CustomerId == customerId);
        }
    }
}
