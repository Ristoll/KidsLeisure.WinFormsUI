using AutoMapper;
using KidsLeisure.BLL.DTO;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;

namespace KidsLeisure.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerDto? CurrentCustomer { get; set; }

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> CreateCustomerAsync()
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();

            var entity = _mapper.Map<CustomerEntity>(CurrentCustomer!);
            await repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            CurrentCustomer!.Id = entity.CustomerId;

            return _mapper.Map<CustomerDto>(entity);
        }


        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();
            var entities = await repository.GetAllAsync();
            return _mapper.Map<List<CustomerDto>>(entities);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var repository = _unitOfWork.GetRepository<CustomerEntity>();
            var entity = await repository.FindAsync(c => c.CustomerId == customerId);
            return entity != null ? _mapper.Map<CustomerDto>(entity) : null;
        }
    }
}
