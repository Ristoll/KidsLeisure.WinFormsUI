using AutoFixture;
using AutoMapper;
using KidsLeisure.BLL.DTO;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Services;
using KidsLeisure.DAL.Entities;
using NSubstitute;
using System.Linq.Expressions;

namespace KidsLeisure.Tests
{
    public class CustomerServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CustomerEntity> _repository;
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IRepository<CustomerEntity>>();
            _mapper = Substitute.For<IMapper>();

            _unitOfWork.GetRepository<CustomerEntity>().Returns(_repository);

            _customerService = new CustomerService(_unitOfWork, _mapper);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldAddEntityAndReturnDto()
        {
            // Arrange
            var dto = _fixture.Create<CustomerDto>();
            var entity = _fixture.Create<CustomerEntity>();

            _mapper.Map<CustomerEntity>(dto).Returns(entity);
            _mapper.Map<CustomerDto>(entity).Returns(dto);
            _repository.AddAsync(entity).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync().Returns(1);

            _customerService.CurrentCustomer = dto;

            // Act
            var result = await _customerService.CreateCustomerAsync();

            // Assert
            await _repository.Received(1).AddAsync(entity);
            await _unitOfWork.Received(1).SaveChangesAsync();
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnMappedList()
        {
            // Arrange
            var entities = _fixture.Create<List<CustomerEntity>>();
            var dtos = _fixture.Create<List<CustomerDto>>();

            _repository.GetAllAsync().Returns(entities);
            _mapper.Map<List<CustomerDto>>(entities).Returns(dtos);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnMappedDto_WhenEntityFound()
        {
            // Arrange
            var entity = _fixture.Create<CustomerEntity>();
            var dto = _fixture.Create<CustomerDto>();

            _repository.FindAsync(Arg.Any<Expression<Func<CustomerEntity, bool>>>()).Returns(entity);
            _mapper.Map<CustomerDto>(entity).Returns(dto);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(entity.CustomerId);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenEntityNotFound()
        {
            // Arrange
            _repository.FindAsync(Arg.Any<Expression<Func<CustomerEntity, bool>>>()).Returns((CustomerEntity?)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(123);

            // Assert
            Assert.Null(result);
        }
    }
}
